using Microsoft.AspNetCore.Mvc;
using PixelGrid_WebApi.Datamodels;
using PixelGrid_WebApi.Services;
using System;
using System.Threading.Tasks;

namespace PixelGrid_WebApi.Controllers
{
    [ApiController]
    [Route("Environment2D/{environmentID}/objects")] 
    public class Object2DController : ControllerBase
    {
        private readonly ISqlObject2DService sqlO2DS;
        private readonly ISqlEnvironment2DService sqlE2DS;
        private readonly IAuthenticationService authService;

        public Object2DController(ISqlObject2DService sqlObject2DService,
                                  ISqlEnvironment2DService sqlEnvironment2DService,
                                  IAuthenticationService authenticationService)
        {
            sqlO2DS = sqlObject2DService;
            sqlE2DS = sqlEnvironment2DService; 
            authService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromRoute] Guid environmentID, [FromBody] Object2D object2D)
        {
            if (environmentID == Guid.Empty)
                return BadRequest("Invalid environmentID");

            var data = await sqlE2DS.GetDataAsync(environmentID);
            if (data == null)
                return NotFound("Environment not found");

            if (data.OwnerUserId != authService.GetCurrentAuthenticatedUserId())
                return Unauthorized("User is not allowed to add the object to the environment");

            object2D.ID = Guid.NewGuid();
            object2D.EnvironmentID = environmentID;

            await sqlO2DS.InsertDataAsync(object2D);

            return Ok("Object2D created successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] Guid environmentID, [FromBody] Object2D object2D)
        {
            if (object2D.ID == Guid.Empty)
                return BadRequest("Invalid ID");

            var data = await sqlE2DS.GetDataAsync(environmentID);
            if (data == null)
                return NotFound("Environment not found");

            if (data.OwnerUserId != authService.GetCurrentAuthenticatedUserId())
                return Unauthorized("User is not allowed to update the object");

            await sqlO2DS.UpdateDataAsync(object2D);

            return Ok("Object2D updated successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] Guid environmentID)
        {
            if (environmentID == Guid.Empty)
                return BadRequest("Invalid GUID");

            var data = await sqlE2DS.GetDataAsync(environmentID);
            if (data == null)
                return NotFound("Environment not found");

            if (data.OwnerUserId != authService.GetCurrentAuthenticatedUserId())
                return Unauthorized("User is not allowed to view the objects");

            var result = await sqlO2DS.GetDataAsync(environmentID);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid environmentID, [FromRoute] Guid id)
        {
            if (environmentID == Guid.Empty || id == Guid.Empty)
                return BadRequest("Invalid ID");

            var data = await sqlE2DS.GetDataAsync(environmentID);
            if (data == null)
                return NotFound("Environment not found");

            if (data.OwnerUserId != authService.GetCurrentAuthenticatedUserId())
                return Unauthorized("User is not allowed to delete the object");

            await sqlO2DS.DeleteDataAsync(environmentID, id);
            return Ok("Object2D deleted successfully.");
        }
    }
}
