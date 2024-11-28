using glissvinyls_plus.Context;
using glissvinyls_plus.Models;
using glissvinyls_plus.Models.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace glissvinyls_plus.Services
{
    public class AcquisitionService
    {
        private readonly AppDbContext _context;

        public AcquisitionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AcquireProductsAsync(AcquisitionRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Verificar si el proveedor existe
                var supplier = await _context.Suppliers.FindAsync(request.SupplierId);
                if (supplier == null)
                    throw new Exception("Supplier not found.");

                // Verificar si el almacén existe
                var warehouse = await _context.Warehouses.FindAsync(request.WarehouseId);
                if (warehouse == null)
                    throw new Exception("Warehouse not found.");

                // Crear la entrada de inventario
                var inventoryEntry = new InventoryEntry
                {
                    EntryDate = DateTime.Now,
                    SupplierId = request.SupplierId,
                    TotalEntry = 0 // Se actualizará más adelante
                };

                _context.InventoryEntries.Add(inventoryEntry);
                await _context.SaveChangesAsync();

                float totalEntry = 0;
                foreach (var productRequest in request.Products)
                {
                    // Verificar si la categoría existe
                    var category = await _context.Categories.FindAsync(productRequest.CategoryId);
                    if (category == null)
                        throw new Exception($"Category with ID {productRequest.CategoryId} not found.");

                    // Buscar si el producto existe por nombre y categoría
                    var product = await _context.Products.FirstOrDefaultAsync(p =>
                        p.Name == productRequest.Name &&
                        p.Category.CategoryId == productRequest.CategoryId);

                    if (product == null)
                    {
                        // Si no existe, crearlo
                        product = new Product
                        {
                            Name = productRequest.Name,
                            Description = productRequest.Description,
                            Price = productRequest.Price,
                            Image = productRequest.Image,
                            Category = category
                        };
                        _context.Products.Add(product);
                        await _context.SaveChangesAsync();
                    }

                    // Crear detalle de entrada
                    var entryDetail = new EntryDetail
                    {
                        EntryId = inventoryEntry.EntryId,
                        ProductId = product.ProductId,
                        Quantity = productRequest.Quantity,
                        PurchasePrice = (float)productRequest.Price,
                        Product = product,
                        InventoryEntry = inventoryEntry
                    };
                    _context.EntryDetails.Add(entryDetail);

                    // Actualizar o crear el stock por almacén
                    var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == product.ProductId && s.WarehouseId == request.WarehouseId);

                    if (stock == null)
                    {
                        stock = new Stock
                        {
                            ProductId = product.ProductId,
                            WarehouseId = request.WarehouseId,
                            AvailableQuantity = productRequest.Quantity,
                            Product = product,
                            Warehouse = warehouse
                        };
                        _context.Stocks.Add(stock);
                    }
                    else
                    {
                        stock.AvailableQuantity += productRequest.Quantity;
                        _context.Stocks.Update(stock);
                    }

                    // Registrar el historial de movimientos
                    var movement = new MovementHistory
                    {
                        ProductId = product.ProductId,
                        MovementDate = DateTime.Now,
                        MovementType = "Entry",
                        Quantity = productRequest.Quantity,
                        WarehouseId = request.WarehouseId,
                        Product = product,
                        Warehouse = warehouse
                    };
                    _context.MovementHistories.Add(movement);

                    // Sumar al total de la entrada
                    totalEntry += (float)(productRequest.Price * productRequest.Quantity);
                }

                // Actualizar total de la entrada
                inventoryEntry.TotalEntry = totalEntry;
                _context.InventoryEntries.Update(inventoryEntry);

                // Guardar todos los cambios
                await _context.SaveChangesAsync();

                // Confirmar transacción
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                // En caso de error, revertir transacción
                await transaction.RollbackAsync();
                throw new Exception($"Error acquiring products: {ex.Message}");
            }
        }

    }
}
