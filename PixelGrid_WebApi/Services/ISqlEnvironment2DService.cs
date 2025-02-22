using PixelGrid_WebApi.Datamodels;

namespace PixelGrid_WebApi.Services
{
    public interface ISqlEnvironment2DService
    {
        Task InsertDataAsync(Environment2D data);
        Task UpdateDataAsync(Environment2D environment);
        Task DeleteDataAsync(Guid id);
        Task<IEnumerable<Environment2D>> GetListOfDataAsync();
    }
}