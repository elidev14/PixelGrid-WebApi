using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using PixelGrid_WebApi.Datamodels;
using PixelGrid_WebApi.Services;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PixelGrid_WebApi.Controllers
{

    [ApiController]
    [Route("Environment2D")]
    public class Object2DController : ControllerBase
    {
        ISqlObject2DService sqlO2DS;
        ISqlEnvironment2DService sqlE2DS;
        IAuthenticationService authService;


        public Object2DController(ISqlObject2DService sqlObject2DService, ISqlEnvironment2DService sqlEnvironment2DService, IAuthenticationService authenticationService)
        {
            sqlO2DS = sqlObject2DService;
            authService = authenticationService;
        }

        [HttpPost("{environmentID}/[controller]")]
        public async Task<IActionResult> Add([FromRoute] Guid environmentID, [FromBody] Object2D object2D)
        {

            if (environmentID == Guid.Empty)
                return BadRequest("Invalid environmentID");

            var data = sqlE2DS.GetDataAsync(environmentID).Result;

            if (data.OwnerUserId != authService.GetCurrentAuthenticatedUserId())
                return Unauthorized("User is not allowed to add the object to the environment");

            object2D.ID = Guid.NewGuid();
            object2D.EnvironmentID = environmentID;

            await sqlO2DS.InsertDataAsync(object2D);

            return Ok("Object2D created successfully.");
        }

        [HttpPut("{environmentID}/[controller]")]
        public async Task<IActionResult> Update([FromRoute] Guid environmentID, Object2D object2D)
        {
            if (object2D.ID == Guid.Empty)
                return BadRequest("Invalid ID");

            var data = sqlE2DS.GetDataAsync(environmentID).Result;

            if (data.OwnerUserId != authService.GetCurrentAuthenticatedUserId())
                return Unauthorized("User is not allowed to update the object");

            await sqlO2DS.UpdateDataAsync(object2D);

            return Ok("Object2D updated");
        }



        [HttpGet("{environmentID}/[controller]")]
        public async Task<IActionResult> Get([FromRoute] Guid environmentID)
        {
            try
            {
                if (environmentID == Guid.Empty)
                    return BadRequest("Invalid GUID");

                var data = sqlE2DS.GetDataAsync(environmentID).Result;

                if (data.OwnerUserId != authService.GetCurrentAuthenticatedUserId())
                    return Unauthorized("User is not allowed to view the object");

                var result = await sqlO2DS.GetDataAsync(environmentID); // Awaiting the task properly

                return Ok(result);
            }
            catch (Exception err)
            {
                return StatusCode(500, $"Internal Server Error: {err.Message}");
            }
        }


        [HttpDelete("{environmentID}/[Controller]/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid environmentID, [FromRoute] Guid id)
        {
            if (environmentID == Guid.Empty || id == Guid.Empty)
                return BadRequest("Invalid ID");

            var data = sqlE2DS.GetDataAsync(environmentID).Result;

            if (data.OwnerUserId != authService.GetCurrentAuthenticatedUserId())
                return Unauthorized("User is not allowed to delete the object");

            await sqlO2DS.DeleteDataAsync(environmentID, id);
            return Ok("Object2D object deleted");
        }
    }
}
