using EindOpdracht.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EindOpdracht.WebApi.Controllers
{
    [ApiController]
    [Route("environment2d")]
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

        //[HttpPost(Name = "PostEnvironment2D")]
        //public ActionResult Add(Environment2D environment2D)
        //{
        //    SqlEnvironment2DRepository.lstEnvironment2D.Add(environment2D);
        //    return StatusCode(201);
        //}

        [HttpPut("{environmentID}", Name = "UpdateEnvironment2D")]
        public async Task<ActionResult> Update(int environmentID, Environment2D newEnvironment2D)
        {
            var existingWeatherForecast = await _sqlEnvironment2DRepository.ReadAsync(environmentID);

            if (existingWeatherForecast == null)
                return NotFound();

            await _sqlEnvironment2DRepository.UpdateAsync(newEnvironment2D);

            return Ok(newEnvironment2D);
        }

        //[HttpPut("{environmentId}", Name = "UpdateEnvironment2D")]
        //public ActionResult Update(int environmentId, Environment2D environment2D)
        //{
        //    if (environmentId != environment2D.Id)
        //    {
        //        return BadRequest("The id of the object did not match the id of the route");
        //    }

        //    Environment2D environmentToChange = GetEnvironment2DToUpdate(environmentId);

        //    if (environmentToChange == null)
        //    {
        //        return NotFound();
        //    }

        //    SqlEnvironment2DRepository.lstEnvironment2D.Remove(environmentToChange);
        //    SqlEnvironment2DRepository.lstEnvironment2D.Add(environment2D);
            
        //    return StatusCode(202);
        //}

        private Environment2D GetEnvironment2DToUpdate(int id)
        {
            foreach (Environment2D item in SqlEnvironment2DRepository.lstEnvironment2D)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }

            return null;
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

        //[HttpDelete("{EnvironmentId}", Name = "DeleteEnvironmentByName")]
        //public IActionResult Delete(int id)
        //{
        //    Environment2D environmentToDelete = GetEnvironment2DToUpdate(id);

        //    if (environmentToDelete == null)
        //    {
        //        return NotFound();
        //    }

        //    SqlEnvironment2DRepository.lstEnvironment2D.Remove(environmentToDelete);

        //    return StatusCode(200);
        //}

        //[HttpDelete("{EnvironmentID}", Name = "DeleteEnvironmentByID")]
        //public async Task<IActionResult> Update(int environmentID)
        //{
        //    var existingWeatherForecast = await _sqlEnvironment2DRepository.ReadAsync(environmentID);

        //    if (existingWeatherForecast == null)
        //        return NotFound();

        //    await _sqlEnvironment2DRepository.DeleteAsync(environmentID);

        //    return Ok();
        //}

        public IActionResult Index()
        {
            return View();
        }
    }
}
