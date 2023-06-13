using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttService.Models
{
    public record MonitoringTopic
    {
        public string MeasurementName { get; set; }

        public string AddinName { get; set; }

        public string TopicName { get; set; }

        public string IdRegex { get; set; }
    }
}
