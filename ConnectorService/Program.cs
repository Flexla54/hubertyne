using ConnectorService;
using ConnectorService.Models;
using Microsoft.EntityFrameworkCore;
using MQTTnet.AspNetCore;

string? dbHostString = Environment.GetEnvironmentVariable("db_host_string");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProvisionRepository, ProvisionRepository>();

builder.Services.AddDbContext<ConnectorDbContext>(options =>
{
    string? connectionString = dbHostString != null
       ? dbHostString + "Database=connector;"
       : builder.Configuration["ConnectionStrings:ConnectorDbContextConnection"];

    options.UseNpgsql(connectionString);
});

builder.Services.AddHostedMqttServer(builder =>
{
    CustomMqttServer.BuildServerOptions(builder);
});

builder.Services.AddMqttConnectionHandler();
builder.Services.AddConnections();

builder.WebHost.UseKestrel(k =>
{
    k.ListenAnyIP(1883, l => l.UseMqtt());

    k.ListenAnyIP(5000);
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

app.UseMqttServer(server =>
{

    server.ValidatingConnectionAsync += CustomMqttServer.BuildCustomConnectionValidator(app.Services);
});

app.Run();

static void ApplyMigration(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ConnectorDbContext>();

    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

