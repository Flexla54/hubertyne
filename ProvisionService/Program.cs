using ProvisionService.Models;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using ProvisionService.Consumers;

string? dbHostString = Environment.GetEnvironmentVariable("db_host_string");
string? rabbitmqSecret = Environment.GetEnvironmentVariable("rabbitmq_secret");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProvisionRepository, ProvisionRepository>();

builder.Services.AddDbContext<ProvisionDbContext>(options =>
{
    string? connectionString = dbHostString != null
       ? dbHostString + "Database=provision;"
       : builder.Configuration["ConnectionStrings:ProvisionDbContextConnection"];

    options.UseNpgsql(connectionString);
});

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumer<ProvisionExistenceConsumer>();
    x.AddConsumer<ObjectConnectedConsumer>();

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

var app = builder.Build();

ApplyMigration(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

static void ApplyMigration(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ProvisionDbContext>();

    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

