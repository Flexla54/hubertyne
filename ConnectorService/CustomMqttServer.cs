using ConnectorService.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Writers;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace ConnectorService
{
    public class CustomMqttServer
    {
        public static void BuildServerOptions(MqttServerOptionsBuilder builder)
        {
            builder.WithDefaultEndpoint();
            builder.WithDefaultEndpointPort(1883);
        }

        public static Func<ValidatingConnectionEventArgs, Task> BuildCustomConnectionValidator(IServiceProvider provider)
        {
            return (c) =>
            {
                var scope = provider.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IProvisionRepository>();

                try
                {
                    Guid id = Guid.Parse(c.UserName);

                    var provision = repo.GetById(id);

                    if (provision == null)
                    {
                        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        c.ReasonString = "Connection id (username) does not exist";
                        return Task.CompletedTask;
                    }

                    if (provision.HasConnected)
                    {
                        c.ReasonCode = MqttConnectReasonCode.Success;
                        return Task.CompletedTask;
                    }

                    repo.HasConnected(id);
                    c.ReasonCode = MqttConnectReasonCode.Success;
                }
                catch (FormatException)
                {
                    c.ReasonCode = MqttConnectReasonCode.PayloadFormatInvalid;
                    c.ReasonString = "Username has to be a Guid";
                }

                return Task.CompletedTask;
            };
        }
    }
}
