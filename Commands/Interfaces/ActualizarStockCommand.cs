using glissvinyls_plus.Context;
using Microsoft.EntityFrameworkCore;

namespace glissvinyls_plus.Commands.Interfaces
{
    public class ActualizarStockCommand : ICommand
    {
        private readonly AppDbContext _context;
        private readonly int _productId;
        private readonly int _warehouseId;
        private readonly int _quantity;

        public ActualizarStockCommand(AppDbContext context, int productId, int warehouseId, int quantity)
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

            if (stock != null)
            {
                stock.AvailableQuantity -= _quantity;
                _context.Stocks.Update(stock);
                await _context.SaveChangesAsync();
            }
        }
    }
}
