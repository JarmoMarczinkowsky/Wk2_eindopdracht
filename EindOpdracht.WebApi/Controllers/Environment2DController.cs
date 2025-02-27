using EindOpdracht.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EindOpdracht.WebApi.Controllers
{
    [ApiController]
    [Route("environment2d")]
    //[Authorize]
    public class Environment2DController : Controller
    {
        private readonly ILogger<Environment2DController> _logger;
        private readonly SqlEnvironment2DRepository _sqlEnvironment2DRepository;
        private readonly IAuthenticationService _authenticationService;
        //private static List<Environment2D> environment2Ds = [];
        public Environment2DController(SqlEnvironment2DRepository sqlEnvironment2DRepository, ILogger<Environment2DController> logger, IAuthenticationService authenticationService)
        {
            _sqlEnvironment2DRepository = sqlEnvironment2DRepository;
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpGet(Name = "GetEnvironment2D")]
        public async Task<ActionResult<IEnumerable<Environment2D>>> Get()
        {
            var environments = await _sqlEnvironment2DRepository.ReadAsync();
            return Ok(environments);
        }

        [HttpGet("{environmentId}", Name = "ReadEnvironment2D")]
        public async Task<ActionResult<Environment2D>> Get(Guid environmentId)
        {
            var environment = await _sqlEnvironment2DRepository.ReadAsync(environmentId);
            var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();

            if(currentUserId == null)
            {
                return NotFound();
            }

            if (environment == null || currentUserId != environment.OwnerUserId)
                return NotFound();

            return Ok(environment);
        }

        [HttpPost(Name = "CreateEnvironment2D")]
        public async Task<ActionResult> Add(Environment2D environment2D)
        {
            var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();

            if(currentUserId == null)
            {
                return NotFound();
            }

            environment2D.Id = Guid.NewGuid();
            environment2D.OwnerUserId = currentUserId;

            var createdEnvironment = await _sqlEnvironment2DRepository.InsertAsync(environment2D);
            return Created();
        }

        [HttpPut("{environmentID}", Name = "UpdateEnvironment2D")]
        public async Task<ActionResult> Update(Guid environmentID, Environment2D newEnvironment2D)
        {
            var existingEnvironment = await _sqlEnvironment2DRepository.ReadAsync(environmentID);
            var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();

            if (existingEnvironment == null || string.IsNullOrWhiteSpace(currentUserId))
                return NotFound();

            newEnvironment2D.Id = environmentID;
            newEnvironment2D.OwnerUserId = currentUserId;
            await _sqlEnvironment2DRepository.UpdateAsync(newEnvironment2D);

            return Ok(newEnvironment2D);
        }

        [HttpDelete("{environmentID}", Name = "DeleteEnvironmentById")]
        public async Task<IActionResult> Update(Guid environmentID)
        {
            var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();

            if (string.IsNullOrWhiteSpace(currentUserId))
                return NotFound();

            var existingEnvironment = await _sqlEnvironment2DRepository.ReadAsync(environmentID);

            if (existingEnvironment == null || currentUserId != existingEnvironment.OwnerUserId)
                return NotFound();

            await _sqlEnvironment2DRepository.DeleteAsync(environmentID);

            return Ok();
        }
    }
}
