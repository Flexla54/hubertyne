using MqttService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttService.Services
{
    public class MonitoringTopicService : IMonitoringTopicService
    {
        private static List<MonitoringTopic> _topics = new();

        public void Add(MonitoringTopic topic)
        {
            _topics.Add(topic);
        }

        public void RemoveByAddinName(string name)
        {
            _topics.RemoveAll(t => t.AddinName == name);
        }

        public ReadOnlyCollection<MonitoringTopic> Topics { get { return new(_topics);  } }
    }
}
