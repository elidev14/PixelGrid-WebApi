using Microsoft.AspNetCore.Mvc;
using PixelGrid_WebApi.Datamodels;
using PixelGrid_WebApi.Services;

namespace PixelGrid_WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class Object2DController : ControllerBase
    {
        ISqlObject2DService sqlO2DS;

        public Object2DController(ISqlObject2DService sqlObject2DService)
        {
            sqlO2DS = sqlObject2DService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(Guid environmentID, Object2D object2D)
        {

            if (environmentID == Guid.Empty) return BadRequest("Invalid environmentID");

            object2D.ID = Guid.NewGuid();
            object2D.EnvironmentID = environmentID;

            await sqlO2DS.InsertDataAsync(object2D);

            return Ok("Object2D created successfully.");
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> Update(Guid guid, Guid environmentID, Object2D object2D)
        {

            if (guid == Guid.Empty) return BadRequest("Invalid GUID");

            object2D.ID = guid;
            object2D.EnvironmentID = environmentID;

            await sqlO2DS.UpdateDataAsync(object2D);

            return Ok("Object2D updated");
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty) return BadRequest("Invalid GUID");

                var result = await sqlO2DS.GetDataAsync(guid); // Awaiting the task properly

                return Ok(result);
            }
            catch (Exception err)
            {
                return StatusCode(500, $"Internal Server Error: {err.Message}");
            }
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            if (guid == Guid.Empty) return BadRequest("Invalid GUID");

            await sqlO2DS.DeleteDataAsync(guid);
            return Ok("Object2D object deleted");
        }
    }
}
