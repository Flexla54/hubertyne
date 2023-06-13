using Microsoft.Extensions.Hosting;
using MassTransit;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using IngestionService.Services;
using IngestionService.PlugAddin;

public class Program
{
    public static async Task Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        string? rabbitmqSecret = Environment.GetEnvironmentVariable("rabbitmq_secret");
        string? influxSecret = Environment.GetEnvironmentVariable("influx_secret");

        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(x => new InfluxDbService(
                    influxSecret ?? "password",
                    influxSecret != null ? "http://influx:1883" : "http://localhost:1883"));

                services.AddMassTransit(x =>
                {
                    x.SetKebabCaseEndpointNameFormatter();

                    x.AddConsumer<MonitoringTopicUpdateConsumer>();

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

                services.AddHostedService<RegisterPlugAddinService>();
            });
    }
}
