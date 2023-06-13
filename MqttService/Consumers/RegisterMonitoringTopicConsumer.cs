using MassTransit;
using Hubertyne.ServiceBus.Contracts.Mqtt;
using ServiceBus.Contracts.Ingestion;
using Microsoft.Extensions.Logging;
using MqttService.Services;
using MqttService.Models;

namespace MqttService.Consumers
{
    public class RegisterMonitoringTopicConsumer : IConsumer<RegisterMonitoringTopic>
    {
        private readonly ILogger<RegisterMonitoringTopicConsumer> _logger;
        private readonly IMonitoringTopicService _topicService;

        public RegisterMonitoringTopicConsumer(ILogger<RegisterMonitoringTopicConsumer> logger, IMonitoringTopicService topicService)
        {
            _logger = logger;
            _topicService = topicService;
        }

        public async Task Consume(ConsumeContext<RegisterMonitoringTopic> context)
        {
            var request = context.Message;

            MonitoringTopic monitoringTopic = new MonitoringTopic()
            {
                AddinName = request.AddinName,
                TopicName = request.TopicName,
                IdRegex = request.IdRegex,
                MeasurementName = request.MeasurementName,
            };

            _topicService.Add(monitoringTopic);

            await context.RespondAsync(new RegisterMonitoringTopicResult() { Success = true });
        }
    }
}
