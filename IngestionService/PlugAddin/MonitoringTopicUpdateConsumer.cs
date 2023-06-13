using InfluxDB.Client.Writes;
using IngestionService.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using ServiceBus.Contracts.Ingestion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngestionService.PlugAddin
{
    public class MonitoringTopicUpdateConsumer : IConsumer<MonitoringTopicUpdate>
    {
        private readonly ILogger<MonitoringTopicUpdate> _logger;
        private readonly InfluxDbService _influx;
        
        public MonitoringTopicUpdateConsumer(ILogger<MonitoringTopicUpdate> logger, InfluxDbService influx)
        {
            _logger = logger;
            _influx= influx;
        }

        public async Task Consume(ConsumeContext<MonitoringTopicUpdate> context)
        {
            var update = context.Message;

            if (update.AddinName != "Plug")
            {
                return;
            }

            _logger.LogInformation($"Writing {update.MeasurementName}, '{update.Payload}'");

            _influx.Write(async write =>
            {
                var point = PointData.Measurement("plug").Tag("id", update.Id.ToString());

                if (update.MeasurementName == "state")
                {
                    point.Field(update.MeasurementName, update.Payload);
                }
                else
                {
                    point.Field(update.MeasurementName, float.Parse(update.Payload, CultureInfo.InvariantCulture.NumberFormat));
                }

                await write.WritePointAsync(point, "plugs", "hubertyne");
                Console.WriteLine($"Written {point}");
            });
        }
    }
}
