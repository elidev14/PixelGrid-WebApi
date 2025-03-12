using Microsoft.AspNetCore.Mvc;
using PixelGrid_WebApi.Datamodels;
using PixelGrid_WebApi.Services;

namespace PixelGrid_WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class Environment2DController : ControllerBase
    {

        ISqlEnvironment2DService sqlE2DS;
        IAuthenticationService authService;

        public Environment2DController(ISqlEnvironment2DService sqlEnvironment2DService, IAuthenticationService authenticationService)
        {
            sqlE2DS = sqlEnvironment2DService;
            authService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddEnvironment2D([FromBody] Environment2D environment)
        {


            if (environment == null)
            {
                return BadRequest("Invalid environment data.");
            }

            string userId = authService.GetCurrentAuthenticatedUserId();

            // Check if the limit has not been exceeded. If it has, return a bad request.
            var existingEnvironments = await sqlE2DS.GetListOfDataAsync(userId);

            if (existingEnvironments.Count() >= 5)
            {
                return BadRequest("You have reached the limit of 5 environments");
            }


            // Check if the environment already exists.
            var existingEnvironment = existingEnvironments
                .FirstOrDefault(e => e.OwnerUserId == userId && e.Name == environment.Name);

            if (existingEnvironment != null)
            {
                return Conflict(new { message = $"An environment with the name '{environment.Name}' already exists." });
            }

            var data = environment;

            data.ID = Guid.NewGuid();
            data.OwnerUserId = userId;

            await sqlE2DS.InsertDataAsync(data);

            return Ok(data);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateEnvironment2D(Environment2D environment)
        {

            if (environment == null)
                return BadRequest("Invalid environment object");


            if (environment.ID == Guid.Empty)
                return BadRequest("Invalid ID");

            if (environment.OwnerUserId != authService.GetCurrentAuthenticatedUserId())
                return Unauthorized("User is not allowed to view the environment");


            await sqlE2DS.UpdateDataAsync(environment);

            return Ok("Environment2D object updated");
        }

        [HttpGet]
        public async Task<IActionResult> GetEnvironment2Ds()
        {
            var result = sqlE2DS.GetListOfDataAsync(authService.GetCurrentAuthenticatedUserId()).Result;

            return Ok(result);
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteEnvironment2D([FromRoute] Guid guid)
        {
            var data = sqlE2DS.GetDataAsync(guid).Result;


            if (data.ID == Guid.Empty)
                return BadRequest("Invalid ID");


            if (data.OwnerUserId != authService.GetCurrentAuthenticatedUserId())
                return Unauthorized("User is not allowed to view the environment");

            await sqlE2DS.DeleteDataAsync(guid);
            return Ok("Environment2D object deleted");
        }

    }
}
