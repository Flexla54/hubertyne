using Hubertyne.ServiceBus.Contracts.Mqtt;
using MassTransit;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace ProvisionService
{
    public class CustomMqttServer : IHostedService
    {
        private MqttServer _server;
        private IRequestClient<CheckProvisionExistence> _requestClient;

        public CustomMqttServer(IRequestClient<CheckProvisionExistence> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var builder = new MqttServerOptionsBuilder();
            builder.WithDefaultEndpoint();
            builder.WithDefaultEndpointPort(1883);

            _server = new MqttFactory().CreateMqttServer(builder.Build());

            _server.ValidatingConnectionAsync += BuildCustomConnectionValidator(_requestClient);

            await _server.StartAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _server.StopAsync();

            return Task.CompletedTask;
        }

        public static void BuildServerOptions(MqttServerOptionsBuilder builder)
        {
            builder.WithDefaultEndpoint();
            builder.WithDefaultEndpointPort(1883);
        }

        public static Func<ValidatingConnectionEventArgs, Task> BuildCustomConnectionValidator(IRequestClient<CheckProvisionExistence> requestClient)
        {
            return async (c) =>
            {
                try
                {
                    Guid id = Guid.Parse(c.UserName);

                    var result = await requestClient.GetResponse<ProvisionExitenceResult>(new { Id = id });

                    var exists = result.Message.Exists;

                    if (!exists)
                    {
                        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        c.ReasonString = "Connection id (username) does not exist";
                        return;
                    }

                    c.ReasonCode = MqttConnectReasonCode.Success;
                    return;

                    // repo.HasConnected(id);
                    // c.ReasonCode = MqttConnectReasonCode.Success;
                }
                catch (FormatException)
                {
                    c.ReasonCode = MqttConnectReasonCode.PayloadFormatInvalid;
                    c.ReasonString = "Username has to be a Guid";
                }
            };
        }
    }
}
