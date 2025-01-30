using glissvinyls_plus.Commands.Interfaces;
using glissvinyls_plus.Commands;
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

                // Comando para crear la salida de inventario
                var crearSalidaCommand = new CrearSalidaInventarioCommand(_context, inventoryExit);
                await crearSalidaCommand.ExecuteAsync();

                float totalExit = 0;
                foreach (var productRequest in request.Products)
                {
                    // Verificar si el producto existe
                    var product = await _context.Products.FindAsync(productRequest.ProductId);
                    if (product == null)
                        throw new Exception($"Product with ID {productRequest.ProductId} not found.");

                    // Comando para verificar el stock
                    var verificarStockCommand = new VerificarStockCommand(_context, product.ProductId, request.WarehouseId, productRequest.Quantity);
                    await verificarStockCommand.ExecuteAsync();

                    // Comando para actualizar el stock
                    var actualizarStockCommand = new ActualizarStockCommand(_context, product.ProductId, request.WarehouseId, productRequest.Quantity);
                    await actualizarStockCommand.ExecuteAsync();

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

                    // Comando para registrar el movimiento
                    var registrarMovimientoCommand = new RegistrarMovimientoCommand(_context, movement);
                    await registrarMovimientoCommand.ExecuteAsync();

                    // Sumar al total de la salida
                    totalExit += (float)(productRequest.SalePrice * productRequest.Quantity);
                }

                // Actualizar total de la salida
                inventoryExit.TotalExit = totalExit;
                _context.InventoryExits.Update(inventoryExit);
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
