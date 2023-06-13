using InfluxDB.Client.Writes;
using IngestionService.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using ServiceBus.Contracts.Ingestion;
using System.Globalization;

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

            _influx.Write(write =>
            {
                PointData point;

                if (update.MeasurementName == "state")
                {
                    point = PointData.Measurement("plug").Tag("id", update.Id.ToString()).Field(update.MeasurementName, update.Payload);
                }
                else
                {
                    point = PointData.Measurement("plug").Tag("id", update.Id.ToString()).Field(update.MeasurementName, float.Parse(update.Payload, CultureInfo.InvariantCulture.NumberFormat));
                }

                write.WritePoint(point, "plugs", "hubertyne");
            });
        }
    }
}
