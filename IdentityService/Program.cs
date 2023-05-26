using IdentityService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using IdentityService.Models;
using Microsoft.AspNetCore.Mvc.Authorization;

string? dbHostString = Environment.GetEnvironmentVariable("db_host_string");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddMvcOptions(options => options.Filters.Add(new AuthorizeFilter("Default")));

// Setup cookie authentication and redirect to the /login page when challenged to authenticate
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        // TODO: Change redirection to production site
        options.LoginPath = "/login";

        var original = options.Events.OnRedirectToLogin;

        options.Events.OnRedirectToLogin = context =>
        {
            if (!context.Request.Path.StartsWithSegments("/connect"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                original(context);
            }

            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Default", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

// Add allow-all CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("*", builder =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
    });

    options.AddPolicy("connectEndpoints", builder =>
    {
        builder.AllowAnyHeader();
        builder.WithMethods("GET", "POST");
        builder.WithOrigins("https://dashboard.localhost", "https://www.hubertyne.me");
    });
});

// Add service which creates test applications
builder.Services.AddHostedService<AuthInitializer>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

// Application DbContext
builder.Services.AddDbContext<UserManagementDbContext>(options =>
{
     string? connectionString = dbHostString != null
        ? dbHostString + "Database=user-management;"
        : builder.Configuration["ConnectionStrings:OpenIddictDbContextConnection"];

    options.UseNpgsql(connectionString);
});

// DbContext for OpenIddict
builder.Services.AddDbContext<OpenIddictDbContext>(options =>
{
    string? connectionString = dbHostString != null
        ? dbHostString + "Database=openiddict;"
        : builder.Configuration["ConnectionStrings:OpenIddictDbContextConnection"];

    options.UseNpgsql(connectionString);
    options.UseOpenIddict();
});

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore().UseDbContext<OpenIddictDbContext>();
    })
    .AddServer(options =>
    {
        // Enable needed endpoints and authorization code flow + PKCE
        options.SetTokenEndpointUris("connect/token");
        options.SetAuthorizationEndpointUris("connect/authorize");
        options.AllowAuthorizationCodeFlow().RequireProofKeyForCodeExchange();

        // TODO: Generate "real" certificates when in production
        options.AddDevelopmentEncryptionCertificate();
        options.AddDevelopmentSigningCertificate();
        options.DisableAccessTokenEncryption(); // TODO: Enable encryption in production

        // We do only support the global "api" scope
        options.RegisterScopes("api");

        options.UseAspNetCore()
            .EnableTokenEndpointPassthrough()
            .EnableAuthorizationEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();

        options.UseAspNetCore();
    });

var app = builder.Build();

// Change the HTTP scheme to HTTPS
// -> This has to be done as TLS termination happens at the ingress
//    and subsequend TCP traffic is only encrypted between the nodes by
//    the linkerd proxies
//
//    TODO: Review
app.Use((context, next) =>
{
    context.Request.Scheme = "https";

    return next(context);
});

app.MapControllers();
app.UseRouting();

app.UseCors("connectEndpoints");

app.UseAuthentication();

app.UseEndpoints((_) => { });

app.Run();

