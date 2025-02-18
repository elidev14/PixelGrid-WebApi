using PixelGrid_WebApi.Datamodels;

namespace PixelGrid_WebApi.Services
{
    public interface ISqlObject2DService
    {
        Task InsertDataAsync(Object2D data);
        Task UpdateDataAsync(Object2D environment);
        Task DeleteDataAsync(Guid id);
        Task<Object2D> GetDataAsync(Guid guid);
        Task<IEnumerable<Object2D>> GetListOfDataAsync();
    }
}