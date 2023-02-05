using IdentityService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Setup cookie authentication and redirect to the /login page when challenged to authenticate
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        // TODO: Change redirection to production site
        options.LoginPath = "/login";
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
});

// Add service which creates test applications
builder.Services.AddHostedService<AuthInitializer>();

// DbContext for OpenIddict
builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseInMemoryDatabase(nameof(DbContext));
    options.UseOpenIddict();
});

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore().UseDbContext<DbContext>();
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

app.UseHttpsRedirection();

app.MapControllers();
app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseEndpoints((_) => { });

// Proxy the AuthSpa so that it can be used form the same origin as the API
// (because of cookie policies)
app.UseSpa(x =>
{
    x.UseProxyToSpaDevelopmentServer("http://localhost:4200/");
});

app.Run();

