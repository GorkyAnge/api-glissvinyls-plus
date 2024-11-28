using glissvinyls_plus.Context;
using glissvinyls_plus.Models;
using glissvinyls_plus.Models.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace glissvinyls_plus.Services
{
    public class SalesService
    {
        private readonly AppDbContext _context;

        public SalesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterSaleAsync(SaleRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Verificar si el almacén existe
                var warehouse = await _context.Warehouses.FindAsync(request.WarehouseId);
                if (warehouse == null)
                    throw new Exception("Warehouse not found.");

                // Crear la salida de inventario
                var inventoryExit = new InventoryExit
                {
                    ExitDate = DateTime.Now,
                    ClientId = request.ClientId,
                    WarehouseId = request.WarehouseId,
                    TotalExit = 0 // Se actualizará más adelante
                };

                _context.InventoryExits.Add(inventoryExit);
                await _context.SaveChangesAsync();

                float totalExit = 0;
                foreach (var productRequest in request.Products)
                {
                    // Verificar si el producto existe
                    var product = await _context.Products.FindAsync(productRequest.ProductId);
                    if (product == null)
                        throw new Exception($"Product with ID {productRequest.ProductId} not found.");

                    // Verificar stock disponible en el almacén
                    var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == product.ProductId && s.WarehouseId == request.WarehouseId);
                    if (stock == null || stock.AvailableQuantity < productRequest.Quantity)
                        throw new Exception($"Insufficient stock for product {product.Name} in warehouse {warehouse.WarehouseName}.");

                    // Crear detalle de salida
                    var exitDetail = new ExitDetail
                    {
                        ExitId = inventoryExit.ExitId,
                        ProductId = product.ProductId,
                        Quantity = productRequest.Quantity,
                        SalePrice = (float)productRequest.SalePrice,
                        Product = product,
                        InventoryExit = inventoryExit
                    };
                    _context.ExitDetails.Add(exitDetail);

                    // Actualizar stock en el almacén
                    stock.AvailableQuantity -= productRequest.Quantity;
                    _context.Stocks.Update(stock);

                    // Registrar el historial de movimientos
                    var movement = new MovementHistory
                    {
                        ProductId = product.ProductId,
                        MovementDate = DateTime.Now,
                        MovementType = "Exit",
                        Quantity = productRequest.Quantity,
                        WarehouseId = request.WarehouseId,
                        Product = product,
                        Warehouse = warehouse
                    };
                    _context.MovementHistories.Add(movement);

                    // Sumar al total de la salida
                    totalExit += (float)(productRequest.SalePrice * productRequest.Quantity);
                }

                // Actualizar total de la salida
                inventoryExit.TotalExit = totalExit;
                _context.InventoryExits.Update(inventoryExit);

                // Guardar todos los cambios
                await _context.SaveChangesAsync();

                // Confirmar transacción
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Revertir transacción en caso de error
                await transaction.RollbackAsync();
                throw new Exception($"Error registering sale: {ex.Message}");
            }
        }
    }
}
