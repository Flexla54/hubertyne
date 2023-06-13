using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus.Contracts.Ingestion
{
    public record DeregisterMonitoringAddin
    {
        public string AddinName { get; set; }
    }
}
