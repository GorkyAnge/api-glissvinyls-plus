using glissvinyls_plus.Models;
using glissvinyls_plus.Models.RequestModels;

namespace glissvinyls_plus.Interfaces
{
    public interface IWarehouseService
    {
        Task<IEnumerable<Warehouse>> GetWarehousesAsync();
        Task<Warehouse> GetWarehouseByIdAsync(int id);
        Task CreateWarehouseAsync(WarehouseRequest request);
        Task UpdateWarehouseAsync(int id, WarehouseRequest request);
        Task DeleteWarehouseAsync(int id);
    }
}
