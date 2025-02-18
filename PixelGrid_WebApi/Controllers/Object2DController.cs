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
        public async Task<IActionResult> Add(Guid environmentID, char prefabID, float posX, float posY,
                                     float scaleX, float scaleY, float rotationZ, int sortingLayer)
        {
            var data = new Object2D
            {
                ID = Guid.NewGuid(),  // Assign a new GUID
                EnvironmentID = environmentID,
                PrefabID = prefabID,
                PosX = posX,
                PosY = posY,
                ScaleX = scaleX,
                ScaleY = scaleY,
                RotationZ = rotationZ,
                SortingLayer = sortingLayer
            };

            await sqlO2DS.InsertDataAsync(data);

            return Ok("Object2D created successfully.");
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> Update(Guid guid, Guid environmentID, char prefabID, float posX, float posY,
                                     float scaleX, float scaleY, float rotationZ, int sortingLayer)
        {
            var data = new Object2D
            {
                ID = guid,  // Assign a new GUID
                EnvironmentID = environmentID,
                PrefabID = prefabID,
                PosX = posX,
                PosY = posY,
                ScaleX = scaleX,
                ScaleY = scaleY,
                RotationZ = rotationZ,
                SortingLayer = sortingLayer
            };

            await sqlO2DS.UpdateDataAsync(data);

            return Ok("Object2D updated");
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid guid)
        {
            try
            {
                var result = await sqlO2DS.GetDataAsync(guid); // Awaiting the task properly

                if (result == null)
                {
                    return NotFound("Object2D not found.");
                }

                var dataObject = new Object2D
                {
                    ID = result.ID,
                    EnvironmentID = result.EnvironmentID,
                    PrefabID = result.PrefabID,
                    PosX = result.PosX,
                    PosY = result.PosY,
                    ScaleX = result.ScaleX,
                    ScaleY = result.ScaleY,
                    RotationZ = result.RotationZ,
                    SortingLayer = result.SortingLayer
                };

                return Ok(dataObject);
            }
            catch (Exception err)
            {
                return StatusCode(500, $"Internal Server Error: {err.Message}");
            }
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            await sqlO2DS.DeleteDataAsync(guid);
            return Ok("Object2D object deleted");
        }
    }
}
