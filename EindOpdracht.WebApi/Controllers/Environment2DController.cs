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
        //private static List<Environment2D> environment2Ds = [];
        public Environment2DController(SqlEnvironment2DRepository sqlEnvironment2DRepository, ILogger<Environment2DController> logger)
        {
            _sqlEnvironment2DRepository = sqlEnvironment2DRepository;
            _logger = logger;
        }

        [HttpGet(Name = "GetEnvironment2D")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            var weatherForecasts = await _sqlEnvironment2DRepository.ReadAsync();
            return Ok(weatherForecasts);
        }

        [HttpGet("{environmentId}", Name = "ReadEnvironment2D")]
        public async Task<ActionResult<WeatherForecast>> Get(int environmentId)
        {
            var weatherForeCast = await _sqlEnvironment2DRepository.ReadAsync(environmentId);
            if (weatherForeCast == null)
                return NotFound();

            return Ok(weatherForeCast);
        }

        [HttpPost(Name = "CreateEnvironment2D")]
        public async Task<ActionResult> Add(Environment2D environment2D)
        {
            //environment2D.Id = Guid.NewGuid();
            environment2D.Id = null;
            var createdWeatherForecast = await _sqlEnvironment2DRepository.InsertAsync(environment2D);
            return Created();
        }

        [HttpPut("{environmentID}", Name = "UpdateEnvironment2D")]
        public async Task<ActionResult> Update(int environmentID, Environment2D newEnvironment2D)
        {
            var existingWeatherForecast = await _sqlEnvironment2DRepository.ReadAsync(environmentID);

            if (existingWeatherForecast == null)
                return NotFound();

            newEnvironment2D.Id = environmentID;
            await _sqlEnvironment2DRepository.UpdateAsync(newEnvironment2D);

            return Ok(newEnvironment2D);
        }

        [HttpDelete("{environmentID}", Name = "DeleteWeatherForecastByDate")]
        public async Task<IActionResult> Update(int environmentID)
        {
            var existingWeatherForecast = await _sqlEnvironment2DRepository.ReadAsync(environmentID);

            if (existingWeatherForecast == null)
                return NotFound();

            await _sqlEnvironment2DRepository.DeleteAsync(environmentID);

            return Ok();
        }
    }
}
