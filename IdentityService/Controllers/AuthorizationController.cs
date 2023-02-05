using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IOpenIddictApplicationManager _applicationManager;

        public AuthorizationController(IOpenIddictApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }

        // Authorization endpoint
        // TODO: Should support POST as this is part of the standard
        [HttpGet("~/connect/authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved");

            // Try to authenticate using the cookie
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                // Challenge the user to authenticate if they are not already logged in
                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                        {
                            RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                        });
            }

            // Defined claims to be added to the token later (Only subject in our case)
            var claims = new List<Claim> {
                new Claim(OpenIddictConstants.Claims.Subject, result.Principal.Identity.Name),
            };

            var claimsIdentity = new ClaimsIdentity(claims, TokenValidationParameters.DefaultAuthenticationType);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Copy scopes from the request
            claimsPrincipal.SetScopes(request.GetScopes());

            // SignIn delegates the actual token generation to OpenIddict here
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Token endpoint
        [EnableCors("*")]
        [HttpPost("~/connect/token")]
        public async Task<IActionResult> Exchange() 
        {
            var request = HttpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved");

            if (request.IsAuthorizationCodeGrantType())
            {
                ClaimsPrincipal claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal!;

                return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            throw new InvalidOperationException("The specified grant type is not supported");
        }
    }
}
