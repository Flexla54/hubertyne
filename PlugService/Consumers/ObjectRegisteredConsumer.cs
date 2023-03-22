using MassTransit;
using Hubertyne.ServiceBus.Contracts.Mqtt;
using PlugService.Models;

namespace PlugService.Consumers
{
    public class ObjectRegisteredConsumer : IConsumer<ObjectRegistered>
    {
        private readonly IPlugRepository plugRepository;

        public ObjectRegisteredConsumer(IPlugRepository plugRepository)
        {
            this.plugRepository = plugRepository;
        }

        public async Task Consume(ConsumeContext<ObjectRegistered> context)
        {
            var id = context.Message.Id;
            var plug = plugRepository.GetbyId(id);

            if (plug != null)
            {
                await plugRepository.ChangeConnectionStatus(id, true);
            } else
            {
                await plugRepository.CreatePlug(new PlugDto() { Description = String.Empty, Name = String.Empty, UserId = Guid.NewGuid() });
            }
        }
    }
}
