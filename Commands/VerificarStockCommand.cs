using glissvinyls_plus.Commands.Interfaces;
using glissvinyls_plus.Context;
using Microsoft.EntityFrameworkCore;

namespace glissvinyls_plus.Commands
{
    public class VerificarStockCommand : ICommand
    {
        private readonly AppDbContext _context;
        private readonly int _productId;
        private readonly int _warehouseId;
        private readonly int _quantity;

        public VerificarStockCommand(AppDbContext context, int productId, int warehouseId, int quantity)
        {
            _context = context;
            _productId = productId;
            _warehouseId = warehouseId;
            _quantity = quantity;
        }

        public async Task ExecuteAsync()
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.ProductId == _productId && s.WarehouseId == _warehouseId);

            if (stock == null || stock.AvailableQuantity < _quantity)
            {
                throw new Exception($"Insufficient stock for product {_productId} in warehouse {_warehouseId}.");
            }
        }
    }
}
