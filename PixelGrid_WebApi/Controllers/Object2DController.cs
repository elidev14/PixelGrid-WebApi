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

        [HttpPost("{environmentID}")]
        public async Task<IActionResult> Add([FromRoute] Guid environmentID, [FromBody] Object2D object2D)
        {

            if (environmentID == Guid.Empty) return BadRequest("Invalid environmentID");

            object2D.ID = Guid.NewGuid();
            object2D.EnvironmentID = environmentID;

            await sqlO2DS.InsertDataAsync(object2D);

            return Ok("Object2D created successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(Object2D object2D)
        {
            if (object2D.ID == Guid.Empty) return BadRequest("Invalid ID");

            await sqlO2DS.UpdateDataAsync(object2D);

            return Ok("Object2D updated");
        }



        [HttpGet("{environmentID}")]
        public async Task<IActionResult> Get([FromRoute] Guid environmentID)
        {
            try
            {
                if (environmentID == Guid.Empty) return BadRequest("Invalid GUID");

                var result = await sqlO2DS.GetDataAsync(environmentID); // Awaiting the task properly

                return Ok(result);
            }
            catch (Exception err)
            {
                return StatusCode(500, $"Internal Server Error: {err.Message}");
            }
        }


        [HttpDelete("{environmentID}/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid environmentID, [FromRoute] Guid id)
        {
            if (environmentID == Guid.Empty || environmentID == Guid.Empty) return BadRequest("Invalid ID");

            await sqlO2DS.DeleteDataAsync(environmentID, id);
            return Ok("Object2D object deleted");
        }
    }
}
