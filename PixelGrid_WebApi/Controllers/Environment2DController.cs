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

        public Environment2DController(ISqlEnvironment2DService sqlEnvironment2DService)
        {
            sqlE2DS = sqlEnvironment2DService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(Environment2D environment)
        {
            var data = environment;
            data.ID = Guid.NewGuid();

            await sqlE2DS.InsertDataAsync(data);

            return Ok("Environment2D object created");
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> Update(Guid guid, Environment2D environment)
        {

            if (guid == Guid.Empty) return BadRequest("Invalid GUID");

            var data = environment;

            data.ID = guid;

            await sqlE2DS.UpdateDataAsync(data);

            return Ok("Environment2D object updated");
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty) return BadRequest("Invalid GUID");

                var result = sqlE2DS.GetDataAsync(guid).Result;

                return Ok(result);
            }
            catch (Exception err)
            {
                return Ok("An error occured.");
            }

        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            if (guid == Guid.Empty) return BadRequest("Invalid GUID");

            await sqlE2DS.DeleteDataAsync(guid);
            return Ok("Environment2D object deleted");
        }

    }
}
