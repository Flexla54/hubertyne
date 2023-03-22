using Hubertyne.ServiceBus.Contracts.Mqtt;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using MqttService.Models;
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

        public CustomMqttServer(IRequestClient<CheckProvisionExistence> requestClient, IPublishEndpoint publishEndpoint, ILogger<CustomMqttServer> logger)
        {
            _checkProvisionExistenceClient = requestClient;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
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
            if (!c.ApplicationMessage.Topic.StartsWith($"shellies/{c.ClientId}/"))
            {
                c.ProcessPublish = false;
                _logger.LogWarning($"Message from {c.ClientId} on {c.ApplicationMessage.Topic} declined");
            }

            await TryHandleAnnouncement(c);
        }

        async Task TryHandleAnnouncement(InterceptingPublishEventArgs c)
        {
            var message = c.ApplicationMessage;

            if (Regex.IsMatch(message.Topic, "^shellies/[a-zA-Z0-9-]+/announce$"))
            {
                try
                {
                    Announcement? announcement = JsonSerializer.Deserialize<Announcement>(System.Text.Encoding.Default.GetString(message.Payload));

                    if (announcement == null)
                    {
                        // Probably some error with the JSON parser
                        throw new JsonException();
                    }

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
        }
    }
}
