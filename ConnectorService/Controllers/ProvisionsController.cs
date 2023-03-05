using ConnectorService.Dtos;
using ConnectorService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConnectorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvisionsController : ControllerBase
    {
        private readonly IProvisionRepository _provisionRepository;

        public ProvisionsController(IProvisionRepository provisionRepository)
        {
            _provisionRepository = provisionRepository;
        }

        [HttpGet]
        public IActionResult GetAllProvisions([FromQuery] Guid? userId)
        {
            if (userId is not null)
            {
                return Ok(_provisionRepository.GetByUserId((Guid) userId));
            }

            return Ok(_provisionRepository.All);
        }

        [HttpPost]
        public IActionResult AddProvision([FromBody] AddProvisionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Provision provision = _provisionRepository.CreateProvision(
                Guid.Parse("0d7dd136-5e82-42f2-bc5c-bb5bfdd13639"),
                dto.Description
            );

            return Ok(provision);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProvision(Guid id)
        {
            _provisionRepository.DeleteProvision(id);

            return NoContent();
        }
    }
}
