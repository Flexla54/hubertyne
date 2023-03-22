using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlugService.Models;

namespace PlugService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlugController : ControllerBase
    {
        private readonly IPlugRepository _repository;
        private readonly ILogger<PlugController> _logger;

        public PlugController(PlugRepository repository, ILogger<PlugController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Plug>> GetAllPlugs()
        {
            try
            {
               return Ok(await _repository.GetAllPlugs());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Plug>> GetByID (Guid id)
        {
            try
            {
                var result = await _repository.GetbyId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Plug>> GetByUserId (Guid id)
        {
            try
            {
                return Ok(await _repository.GetbyUserId(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Plug>> AddPlug(PlugDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var plug = await _repository.CreatePlug(dto);

                _logger.LogInformation("New Plug created with id {PlugID}", plug.Id);

                return Ok(plug);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<Plug>> ChangeName(Guid id, [FromBody] UpdatePlugDto dto)
        {
            try
            {
                var plug = await _repository.ChangeName(id, dto);


                if (plug == null)
                {
                    return NotFound();
                }
                _logger.LogInformation($"Change name: {plug.Name}");

                return Ok(plug);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
