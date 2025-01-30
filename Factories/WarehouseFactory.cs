using glissvinyls_plus.Factories.Interfaces;
using glissvinyls_plus.Models;

namespace glissvinyls_plus.Factories
{
    public class WarehouseFactory : IWarehouseFactory
    {
        public Warehouse CreateWarehouse(string warehouseName, string address)
        {
            return new Warehouse
            {
                WarehouseName = warehouseName,
                Address = address
            };
        }
    }
}
