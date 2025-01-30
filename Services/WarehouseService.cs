using glissvinyls_plus.Context;
using glissvinyls_plus.Factories.Interfaces;
using glissvinyls_plus.Interfaces;
using glissvinyls_plus.Models.RequestModels;
using glissvinyls_plus.Models;
using Microsoft.EntityFrameworkCore;

public class WarehouseService : IWarehouseService
{
    private readonly AppDbContext _context;
    private readonly IWarehouseFactory _warehouseFactory;

    public WarehouseService(AppDbContext context, IWarehouseFactory warehouseFactory)
    {
        _context = context;
        _warehouseFactory = warehouseFactory;
    }

    public async Task<IEnumerable<Warehouse>> GetWarehousesAsync()
    {
        return await _context.Warehouses
            .Select(w => new Warehouse
            {
                WarehouseId = w.WarehouseId,
                WarehouseName = w.WarehouseName,
                Address = w.Address
            })
            .ToListAsync();
    }

    public async Task<Warehouse> GetWarehouseByIdAsync(int id)
    {
        return await _context.Warehouses.FirstOrDefaultAsync(w => w.WarehouseId == id);
    }

    public async Task CreateWarehouseAsync(WarehouseRequest request)
    {
        // Usar el Factory para crear un Warehouse
        var warehouse = _warehouseFactory.CreateWarehouse(request.WarehouseName, request.Address);

        _context.Warehouses.Add(warehouse);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateWarehouseAsync(int id, WarehouseRequest request)
    {
        var warehouse = await _context.Warehouses.FindAsync(id);
        if (warehouse == null)
        {
            throw new Exception("Warehouse not found");
        }

        warehouse.WarehouseName = request.WarehouseName;
        warehouse.Address = request.Address;

        _context.Entry(warehouse).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteWarehouseAsync(int id)
    {
        var warehouse = await _context.Warehouses.FindAsync(id);
        if (warehouse == null)
        {
            throw new Exception("Warehouse not found");
        }

        _context.Warehouses.Remove(warehouse);
        await _context.SaveChangesAsync();
    }
}