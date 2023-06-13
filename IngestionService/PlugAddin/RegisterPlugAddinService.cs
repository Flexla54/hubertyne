using MassTransit;
using MassTransit.Internals;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceBus.Contracts.Ingestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngestionService.PlugAddin
{
    public class RegisterPlugAddinService : IHostedService
    {
        private IRequestClient<RegisterMonitoringTopic> _registerRequestClient;
        private IRequestClient<DeregisterMonitoringAddin> _deregisterRequestClient;
        private ILogger<RegisterPlugAddinService> _logger;

        public RegisterPlugAddinService(IRequestClient<RegisterMonitoringTopic> requestClient, ILogger<RegisterPlugAddinService> logger)
        {
            _registerRequestClient = requestClient;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Register the plug messages

            RegisterMonitoringTopic registerPowerTopic = new RegisterMonitoringTopic()
            {
                AddinName = "Plug",
                MeasurementName = "power",
                IdRegex = "^shellies\\/([^\\/]+)\\/relay\\/0\\/power",
                TopicName = "power"
            };

            RegisterMonitoringTopic registerEnergyTopic = new RegisterMonitoringTopic()
            {
                AddinName = "Plug",
                MeasurementName = "energy",
                IdRegex = "^shellies\\/([^\\/]+)\\/relay\\/0\\/energy",
                TopicName = "energy"
            };

            RegisterMonitoringTopic registerStateTopic = new RegisterMonitoringTopic()
            {
                AddinName = "Plug",
                MeasurementName = "state",
                IdRegex = "^shellies\\/([^\\/]+)\\/relay\\/0",
                TopicName = "relay\\/0"
            };

            RegisterMonitoringTopic registerTemperatureTopic = new RegisterMonitoringTopic()
            {
                AddinName = "Plug",
                MeasurementName = "temperature",
                IdRegex = "^shellies\\/([^\\/]+)\\/temperature",
                TopicName = "temperature",
            };

            RegisterMonitoringTopic registerOvertemperatureTopic = new RegisterMonitoringTopic()
            {
                AddinName = "Plug",
                MeasurementName = "overtemperature",
                IdRegex = "^shellies\\/([^\\/]+)\\/overtemperature",
                TopicName = "relay\\/0\\/power"
            };

            await _registerRequestClient.GetResponse<RegisterMonitoringTopicResult>(registerPowerTopic).OrTimeout(10000);
            await _registerRequestClient.GetResponse<RegisterMonitoringTopicResult>(registerEnergyTopic).OrTimeout(10000);
            await _registerRequestClient.GetResponse<RegisterMonitoringTopicResult>(registerStateTopic).OrTimeout(10000);
            await _registerRequestClient.GetResponse<RegisterMonitoringTopicResult>(registerTemperatureTopic).OrTimeout(10000);
            await _registerRequestClient.GetResponse<RegisterMonitoringTopicResult>(registerOvertemperatureTopic).OrTimeout(10000);

            _logger.LogInformation("All plug monitoring topics successfully registered");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var response = await _deregisterRequestClient
                .GetResponse<DeregisterMonitoringAddinResult>(new() { AddinName = "Plug" });

            if (!response.Message.Success)
            {
                _logger.LogError("Error deregistering plug addin topics");
            }
            else
            {
                _logger.LogInformation("Plug addin topics successfully deregistered");
            }
        }
    }
}
