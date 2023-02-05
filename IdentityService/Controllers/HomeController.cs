using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [Route("/api/[controller]")]
        public ActionResult Index()
        {
            var data = new { message = User.Identity?.Name ?? "Not logged in" };

            return new JsonResult(data);
        }
    }
}
