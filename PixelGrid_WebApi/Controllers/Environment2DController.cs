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
        public async Task<IActionResult> Add(double maxHeight, double maxLength, string name, string ownerUserId)
        {
            var data = new Environment2D { ID = Guid.NewGuid(), MaxHeight = maxHeight, MaxLength = maxLength, Name = name, OwnerUserId = ownerUserId };

            await sqlE2DS.InsertDataAsync(data);

            return Ok("Environment2D object created");
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> Update(Guid guid, double maxHeight, double maxLength, string name, string ownerUserId)
        {
            var data = new Environment2D { ID = guid, MaxHeight = maxHeight, MaxLength = maxLength, Name = name, OwnerUserId = ownerUserId };

            await sqlE2DS.UpdateDataAsync(data);

            return Ok("Environment2D object updated");
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid guid)
        {
            var result = sqlE2DS.GetDataAsync(guid).Result;

            try
            {
                var dataObject = new Environment2D { Name = result.Name, OwnerUserId = result.OwnerUserId, MaxHeight = result.MaxHeight, MaxLength = result.MaxLength };

                return Ok(dataObject);
            }
            catch (Exception err)
            {
                return Ok("An error occured.");
            }

        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            await sqlE2DS.DeleteDataAsync(guid);
            return Ok("Environment2D object deleted");
        }

    }
}
