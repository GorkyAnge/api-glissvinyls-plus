using glissvinyls_plus.Models;

namespace glissvinyls_plus.Factories.Interfaces
{
    public interface IWarehouseFactory
    {
        Warehouse CreateWarehouse(string warehouseName, string address);
    }
}
