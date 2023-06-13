using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Contracts.Ingestion
{
    public record MonitoringTopicUpdate
    {
        public string MeasurementName { get; set; }

        public string AddinName { get; set; }

        public Guid Id { get; set; }

        public string Payload { get; set; }
    }
}
