﻿using EindOpdracht.WebApi.Services;
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
        private readonly IEnvironment2DRepository _sqlEnvironment2DRepository;
        private readonly IAuthenticationService _authenticationService;

        public Environment2DController(IEnvironment2DRepository sqlEnvironment2DRepository, ILogger<Environment2DController> logger, IAuthenticationService authenticationService)
        {
            _sqlEnvironment2DRepository = sqlEnvironment2DRepository;
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpGet(Name = "GetEnvironment2D")]
        public async Task<ActionResult<IEnumerable<Environment2D>>> Get()
        {
            var currentUserId = _authenticationService.GetCurrentAuthenticatedUserId();

            if(currentUserId == null)
            {
                return Unauthorized();
            }

            var environments = await _sqlEnvironment2DRepository.ReadWorldsFromUserAsync(currentUserId);
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
                return Unauthorized();
            }

            var environmentsByUser = await _sqlEnvironment2DRepository.ReadWorldsFromUserAsync(currentUserId);
            int userEnvironmentCount = environmentsByUser.Count();
            if(userEnvironmentCount >= 10)
            {
                return BadRequest("User has too many worlds");
            }
            
            foreach(var environments in environmentsByUser)
            {
                if(environments.Name == environment2D.Name)
                {
                    return BadRequest("World with name already exists");
                }
            }

            environment2D.Id = Guid.NewGuid();
            environment2D.OwnerUserId = currentUserId;

            var createdEnvironment = await _sqlEnvironment2DRepository.InsertAsync(environment2D);
            return CreatedAtRoute("ReadEnvironment2D", new {environmentId = createdEnvironment.Id}, createdEnvironment);
        }

        /// <summary>
        /// For reading objects associated by the level
        /// </summary>
        /// <param name="environmentId"></param>
        /// <returns></returns>
        [HttpGet("{environmentId}/objects", Name = "ReadAllObjectsByEnvironment")]
        public async Task<ActionResult<Object2D>> GetObjectsByEnvironmentId(Guid environmentId)
        {
            var objects = await _sqlEnvironment2DRepository.ReadObjectsByEnvironment(environmentId);
            return Ok(objects);
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
        public async Task<IActionResult> Delete(Guid environmentID)
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
