using MassTransit;
using Hubertyne.ServiceBus.Contracts.Mqtt;
using PlugService.Models;
using EntityFramework.Exceptions.Common;

namespace PlugService.Consumers
{
    public class ObjectRegisteredConsumer : IConsumer<ObjectRegistered>
    {
        private readonly IPlugRepository plugRepository;
        private readonly ILogger<ObjectRegisteredConsumer> _logger;

        public ObjectRegisteredConsumer(IPlugRepository plugRepository, ILogger<ObjectRegisteredConsumer> logger)
        {
            this.plugRepository = plugRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ObjectRegistered> context)
        {
            // Only handle the message if the Model is "SHPLG-S" (ergo a Shelly Plug-S)
            if (context.Message.Model != "SHPLG-S") { return; }

            _logger.LogInformation($"Creating new Shelly Plug-S with id {context.Message.Id}");

            try
            {
                await plugRepository.CreatePlug(context.Message.Id, context.Message.ConnectedDate);
            } catch (UniqueConstraintException e)
            {
                _logger.LogInformation($"Shelly Plug-S with id {context.Message.Id} already exists");
            }
        }
    }
}
