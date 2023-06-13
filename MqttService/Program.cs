using ProvisionService;
using Microsoft.Extensions.Hosting;
using MassTransit;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MqttService.Services;
using MqttService.Consumers;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        string? rabbitmqSecret = Environment.GetEnvironmentVariable("rabbitmq_secret");

        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.SetKebabCaseEndpointNameFormatter();

                    x.AddConsumer<RegisterMonitoringTopicConsumer>();

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(rabbitmqSecret != null ? "rabbitmq" : "localhost", "/", h =>
                        {
                            h.Username(rabbitmqSecret != null ? "hubertyne" : "guest");
                            h.Password(rabbitmqSecret ?? "guest");
                        });

                        cfg.ConfigureEndpoints(context);
                    });
                });

                services.AddSingleton<IMonitoringTopicService, MonitoringTopicService>();
                services.AddHostedService<CustomMqttServer>();
            });
    }
}

