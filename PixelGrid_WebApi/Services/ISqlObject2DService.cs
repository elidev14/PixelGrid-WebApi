using PixelGrid_WebApi.Datamodels;

namespace PixelGrid_WebApi.Services
{
    public interface ISqlObject2DService
    {
        Task InsertDataAsync(Object2D object2D);
        Task UpdateDataAsync(Object2D object2D);
        Task DeleteDataAsync(Guid environmentID, Guid id);
        Task<IEnumerable<Object2D>> GetDataAsync(Guid environmentID);
    }
}