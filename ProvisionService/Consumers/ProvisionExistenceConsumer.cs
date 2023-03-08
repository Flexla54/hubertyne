using Hubertyne.ServiceBus.Contracts.Mqtt;
using MassTransit;
using ProvisionService.Models;

namespace ProvisionService.Consumers
{
    public class ProvisionExistenceConsumer : IConsumer<CheckProvisionExistence>
    {
        private readonly IProvisionRepository provisionRepository;

        public ProvisionExistenceConsumer(IProvisionRepository provisionRepository)
        {
            this.provisionRepository = provisionRepository;
        }

        public async Task Consume(ConsumeContext<CheckProvisionExistence> context)
        {
            var provision = provisionRepository.GetById(context.Message.Id);

            await context.RespondAsync<ProvisionExitenceResult>(new { Exists = provision != null });
        }
    }
}
