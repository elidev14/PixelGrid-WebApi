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

        [HttpPost]
        public async Task<IActionResult> AddEnvironment2D([FromBody] Environment2D environment)
        {
            var data = environment;
            data.ID = Guid.NewGuid();

            await sqlE2DS.InsertDataAsync(data);

            return Ok("Environment2D object created");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEnvironment2D(Environment2D environment)
        {
            if (environment.ID == Guid.Empty) return BadRequest("Invalid ID");

            await sqlE2DS.UpdateDataAsync(environment);

            return Ok("Environment2D object updated");
        }

        [HttpGet]
        public async Task<IActionResult> GetEnvironment2Ds()
        {
            try
            {

                var result = sqlE2DS.GetListOfDataAsync().Result;

                return Ok(result);
            }
            catch (Exception err)
            {
                return Ok("An error occured.");
            }

        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteEnvironment2D([FromRoute] Guid guid)
        {
            if (guid == Guid.Empty) return BadRequest("Invalid GUID");

            await sqlE2DS.DeleteDataAsync(guid);
            return Ok("Environment2D object deleted");
        }

    }
}
