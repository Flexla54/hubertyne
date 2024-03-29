﻿using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace IdentityService
{
    // Service class to initialize the OpenIddict applications
    public class AuthInitializer : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public AuthInitializer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();

            var userManagementContext = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();
            var openIddictContext = scope.ServiceProvider.GetRequiredService<OpenIddictDbContext>();

            if (userManagementContext.Database.GetPendingMigrations().Any())
            {
                userManagementContext.Database.Migrate();
            }

            if (openIddictContext.Database.GetPendingMigrations().Any())
            {
                openIddictContext.Database.Migrate();
            }

            await openIddictContext.Database.EnsureCreatedAsync(cancellationToken);

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            // Debug application
            if (await manager.FindByClientIdAsync("debug", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "debug",
                    DisplayName = "Debug application (removed in production)",
                    RedirectUris = { new Uri("about:blank"), new Uri("https://oauthdebugger.com/debug") },
                    Type = OpenIddictConstants.ClientTypes.Public,
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                        OpenIddictConstants.Permissions.ResponseTypes.Code
                    }
                }, cancellationToken);

                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "hubertyne-spa",
                    DisplayName = "Hubertyne Client SPA (Dashboard)",
                    RedirectUris = { new Uri("http://localhost:4200/home"), new Uri("https://oauthdebugger.com/debug"), new Uri("https://www.hubertyne.me/home") },
                    Type = OpenIddictConstants.ClientTypes.Public,
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                        OpenIddictConstants.Permissions.ResponseTypes.Code,
                    }
                });
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
