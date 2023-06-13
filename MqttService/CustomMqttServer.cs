using Hubertyne.ServiceBus.Contracts.Mqtt;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using MqttService.Models;
using MqttService.Services;
using ServiceBus.Contracts.Ingestion;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProvisionService
{
    public class CustomMqttServer : IHostedService
    {
        private MqttServer? _server;
        private IRequestClient<CheckProvisionExistence> _checkProvisionExistenceClient;
        private IPublishEndpoint _publishEndpoint;
        private ILogger<CustomMqttServer> _logger;
        private IMonitoringTopicService _topicService;

        public CustomMqttServer(IRequestClient<CheckProvisionExistence> requestClient, IPublishEndpoint publishEndpoint, ILogger<CustomMqttServer> logger,
            IMonitoringTopicService topicService)
        {
            _checkProvisionExistenceClient = requestClient;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _topicService = topicService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var builder = new MqttServerOptionsBuilder();
            builder.WithDefaultEndpoint();
            builder.WithDefaultEndpointPort(1883);

            _server = new MqttFactory().CreateMqttServer(builder.Build());

            _server.ValidatingConnectionAsync += CustomValidatingConnectionAsync;
            _server.InterceptingSubscriptionAsync += CustomInterceptingSubscriptionAsync;
            _server.InterceptingPublishAsync += CustomInterceptingPublishAsync;

            await _server.StartAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _server.StopAsync();

            return Task.CompletedTask;
        }

        public static void BuildServerOptions(MqttServerOptionsBuilder builder)
        {
            builder.WithDefaultEndpoint();
            builder.WithDefaultEndpointPort(1883);
        }

        async Task CustomValidatingConnectionAsync(ValidatingConnectionEventArgs c)
        {
            if (c.UserName != c.ClientId)
            {
                c.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
                c.ReasonString = "Connection id (username) does not match client id";
                return;
            }

            try
            {
                Guid id = Guid.Parse(c.UserName);

                var result = await _checkProvisionExistenceClient.GetResponse<ProvisionExitenceResult>(new { Id = id });

                var exists = result.Message.Exists;

                if (!exists)
                {
                    c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                    c.ReasonString = "Connection id (username) does not exist";

                    _logger.LogWarning($"Unknown id (username) '{id}' was refused");

                    return;
                }

                c.ReasonCode = MqttConnectReasonCode.Success;

                _logger.LogInformation($"Id (username) '{id} connected");

                return;
            }
            catch (FormatException)
            {
                c.ReasonCode = MqttConnectReasonCode.PayloadFormatInvalid;
                c.ReasonString = "Username has to be a Guid";
            }
        }

        Task CustomInterceptingSubscriptionAsync(InterceptingSubscriptionEventArgs c)
        {
            _logger.LogInformation($"Subscription: {c.TopicFilter.Topic}");

            if (!c.TopicFilter.Topic.StartsWith($"shellies/{c.ClientId}/"))
            {
                _logger.LogWarning($"Subscription for ({c.TopicFilter.Topic}) from {c.ClientId} declined");
                c.ProcessSubscription = false;
            }

            return Task.CompletedTask;
        }

        async Task CustomInterceptingPublishAsync(InterceptingPublishEventArgs c)
        {
            string topic = c.ApplicationMessage.Topic;

            MatchCollection matches = Regex.Matches(topic, "^shellies\\/[^\\/]+\\/(.*[^/])");

            if (matches.Count < 0)
            {
                c.ProcessPublish = false;
                _logger.LogWarning($"Message from {c.ClientId} on {c.ApplicationMessage.Topic} declined");
            }

            string shellyTopic = matches[0].Groups[1].Value;

            if (shellyTopic == "announcement")
            {
                await HandleAnnouncement(c).ConfigureAwait(false);
            }

            await HandleMonitoringTopic(c, shellyTopic).ConfigureAwait(false);
        }

        async Task HandleAnnouncement(InterceptingPublishEventArgs c)
        {
            var message = c.ApplicationMessage;

            try
            {
                Announcement? announcement = JsonSerializer.Deserialize<Announcement>(
                    System.Text.Encoding.Default.GetString(message.Payload),
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );

                if (announcement == null || announcement.Id == null || announcement.Model == null)
                {
                    // User did not provide mandatory types
                    throw new JsonException();
                }

                _logger.LogInformation($"Announcement from {announcement.Model}");

                ObjectRegistered publishMessage = new() { Id = announcement.Id, Model = announcement.Model, ConnectedDate = DateTime.UtcNow };

                await _publishEndpoint.Publish(publishMessage);
            }
            catch (JsonException)
            {
                c.CloseConnection = true;
                _logger.LogError($"Received invalid JSON from {c.ClientId}; Disconnecting");

                return;
            }
        }

        async Task HandleMonitoringTopic(InterceptingPublishEventArgs c, string processedTopic)
        {

            List<MonitoringTopic> topics = _topicService.Topics.Where(t => t.TopicName == processedTopic).ToList();

            foreach (MonitoringTopic topic in topics)
            {
                _logger.LogInformation($"Found fitting measurement: {topic.MeasurementName}");

                MonitoringTopicUpdate update = new MonitoringTopicUpdate()
                {
                    AddinName = topic.AddinName,
                    Id = new Guid(c.ClientId),
                    MeasurementName = topic.MeasurementName,
                    Payload = System.Text.Encoding.Default.GetString(c.ApplicationMessage.Payload),
                };

                await _publishEndpoint.Publish(update);
            }
        }
    }
}
