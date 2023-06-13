using MqttService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttService.Services
{
    public interface IMonitoringTopicService
    {
        public void Add(MonitoringTopic topic);

        public void RemoveByAddinName(string name);

        public ReadOnlyCollection<MonitoringTopic> Topics { get; }
    }
}
