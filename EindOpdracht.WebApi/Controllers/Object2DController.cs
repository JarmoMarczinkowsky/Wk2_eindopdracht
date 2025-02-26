using EindOpdracht.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EindOpdracht.WebApi.Controllers
{
    [ApiController]
    [Route("object2d")]
    public class Object2DController : Controller
    {
        private readonly ILogger<Environment2DController> _logger;
        private readonly SqlObject2DRepository _object2DRepository;
        public Object2DController(SqlObject2DRepository object2DRepository, ILogger<Environment2DController> logger)
        {
             _object2DRepository = object2DRepository;
            _logger = logger;
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

        [HttpGet("{environmentId}/objects", Name = "ReadAllObjectsByEnvironment")]
        public async Task<ActionResult<Object2D>> GetByEnvironmentId(Guid environmentId)
        {
            var objects = await _object2DRepository.ReadByEnvironmentIdAsync(environmentId);
            return Ok(objects);
        }

        [HttpPost(Name = "CreateObject2D")]
        public async Task<ActionResult> Add(Object2D object2D)
        {
            //environment2D.Id = Guid.NewGuid();
            object2D.Id = Guid.NewGuid();
            var createdObject = await _object2DRepository.InsertAsync(object2D);
            return Created();
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
        public async Task<IActionResult> Update(Guid environmentID)
        {
            var existingObject = await _object2DRepository.ReadAsync(environmentID);

            if (existingObject == null)
                return NotFound();

            await _object2DRepository.DeleteAsync(environmentID);

            return Ok();
        }
    }
}
