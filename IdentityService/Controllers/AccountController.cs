using EntityFramework.Exceptions.Common;
using IdentityService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Signup([FromBody] SignupDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Conflict(new JsonResult(new { title = "You can't create a second account" }));
            }

            try
            {
                User user = _userRepository.CreateUser(dto.Username, dto.Email, dto.Password);

                await SignInAsync(user.Username, user.Id);

                return Ok(user);
            } 
            catch (UniqueConstraintException)
            {
                return Conflict(new { title = "User with this username or email already exists" });
            }
        }

        [HttpPost("session")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Conflict(new JsonResult(new { title = "You can't login again without logging out first" }));
            }

            var result = _userRepository.VerifyUser(dto.Username, dto.Password);

            if (result is null)
            {
                return Unauthorized();
            }

            await SignInAsync(result.Username, result.Id);

            return NoContent();
        }

        [HttpDelete("session")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return NoContent();
        }

        private async Task SignInAsync(string username, Guid user_id)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, username),
                new Claim("user_id", user_id.ToString()) 
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
        }
    }
}
