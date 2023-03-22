using MassTransit;
using ProvisionService.Models;
using Hubertyne.ServiceBus.Contracts.Mqtt;

namespace ProvisionService.Consumers
{
    public class ObjectConnectedConsumer : IConsumer<ObjectConnected>
    {
        private readonly IProvisionRepository provisionRepository;

        public ObjectConnectedConsumer(IProvisionRepository provisionRepository)
        {
            this.provisionRepository = provisionRepository;
        }

        public Task Consume(ConsumeContext<ObjectConnected> context)
        {
            provisionRepository.HasConnected(context.Message.Id);

            return Task.CompletedTask;
        }
    }
}
