using EindOpdracht.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EindOpdracht.WebApi.Controllers
{
    [ApiController]
    [Route("object2d")]
    public class Object2DController : Controller
    {
        private readonly ILogger<Object2DController> _logger;
        private readonly IObject2dRepository _object2DRepository;
        private readonly IAuthenticationService _authenticationService;
        public Object2DController(IObject2dRepository object2DRepository, ILogger<Object2DController> logger, IAuthenticationService authenticationService)
        {
             _object2DRepository = object2DRepository;
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpGet("objects", Name = "ReadAllObjects")]
        public async Task<ActionResult<IEnumerable<Object2D>>> Get()
        {
            var objects = await _object2DRepository.ReadAsync();
            return Ok(objects);
        }

        [HttpGet("objects/{objectId}", Name = "ReadSingleObject")]
        public async Task<ActionResult<Object2D>> Get(Guid objectId)
        {
            var objects = await _object2DRepository.ReadAsync(objectId);
            if (objects == null)
                return NotFound();

            return Ok(objects);
        }

        //[HttpGet("{environmentId}/objects", Name = "ReadAllObjectsByEnvironment")]
        //public async Task<ActionResult<Object2D>> GetByEnvironmentId(Guid environmentId)
        //{
        //    var objects = await _object2DRepository.ReadObjectsByEnvironment(environmentId);
        //    return Ok(objects);
        //}

        [HttpPost("create", Name = "CreateObject2D")]
        public async Task<ActionResult> Add(Object2D object2D)
        {
            var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();

            if (currentUserId == null)
            {
                return Unauthorized();
            }

            //environment2D.Id = Guid.NewGuid();
            object2D.Id = Guid.NewGuid();
            var createdObject = await _object2DRepository.InsertAsync(object2D);
            return CreatedAtRoute("CreateObject2D", new { id = createdObject.Id }, createdObject);
        }

        [HttpPut("{environmentID}", Name = "UpdateObject2D")]
        public async Task<ActionResult> Update(Guid objectID, Object2D newObject2D)
        {
            var existingObject = await _object2DRepository.ReadAsync(objectID);

            if (existingObject == null)
                return NotFound();

            newObject2D.Id = objectID;
            await _object2DRepository.UpdateAsync(newObject2D);

            return Ok(newObject2D);
        }

        [HttpDelete("{objectID}", Name = "DeleteObjectsById")]
        public async Task<IActionResult> Delete(Guid environmentID)
        {
            var existingObject = await _object2DRepository.ReadAsync(environmentID);

            if (existingObject == null)
                return NotFound();

            await _object2DRepository.DeleteAsync(environmentID);

            return Ok();
        }
    }
}
