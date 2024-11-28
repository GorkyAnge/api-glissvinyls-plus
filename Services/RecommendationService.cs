using glissvinyls_plus.Context;
using glissvinyls_plus.Models;
using Microsoft.EntityFrameworkCore;

namespace glissvinyls_plus.Services
{
    public class RecommendationService
    {
        private readonly AppDbContext _context;

        public RecommendationService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<(Product, int)>> GetTopSellingProductsAsync()
        {
            var sales = await _context.ExitDetails
                .GroupBy(ed => ed.ProductId)
                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(ed => ed.Quantity) })
                .OrderByDescending(s => s.TotalSold)
                .ToListAsync();

            var products = new List<(Product, int)>();

            foreach (var sale in sales)
            {
                var product = await _context.Products.FindAsync(sale.ProductId);
                if (product != null)
                    products.Add((product, sale.TotalSold));
            }

            return products;
        }

        // Predicción de necesidades de stock

        public async Task<List<(Product, int)>> PredictStockNeedsAsync(int months)
        {
            var today = DateTime.Today;
            var pastData = await _context.ExitDetails
                .Include(ed => ed.InventoryExit)
                .Where(ed => ed.InventoryExit.ExitDate >= today.AddMonths(-months)) 
                .GroupBy(ed => new { ed.ProductId, Month = ed.InventoryExit.ExitDate.Month })
                .Select(g => new
                {
                    g.Key.ProductId,
                    g.Key.Month,
                    TotalSold = g.Sum(ed => ed.Quantity)
                })
                .ToListAsync();

            var predictions = new List<(Product, int)>();

            var productGroups = pastData
                .GroupBy(p => p.ProductId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var productId in productGroups.Keys)
            {
                var sales = productGroups[productId]
                    .OrderBy(s => s.Month)
                    .Select(s => s.TotalSold)
                    .ToList();

                if (sales.Count >= 3)
                {
                    var lastThreeMonths = sales.Skip(Math.Max(0, sales.Count - 3)).ToList();
                    var predictedDemand = (int)Math.Round(lastThreeMonths.Average());

                    var product = await _context.Products.FindAsync(productId);
                    if (product != null)
                    {
                        predictions.Add((product, predictedDemand));
                    }
                }
            }

            return predictions;
        }

        public async Task<List<(Product, int)>> GetTopSellingProductsByWarehouseAsync(int warehouseId)
        {
            var sales = await _context.ExitDetails
                .Include(ed => ed.InventoryExit)
                .Where(ed => ed.InventoryExit.WarehouseId == warehouseId)
                .GroupBy(ed => ed.ProductId)
                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(ed => ed.Quantity) })
                .OrderByDescending(s => s.TotalSold)
                .Take(10)
                .ToListAsync();

            var products = new List<(Product, int)>();

            foreach (var sale in sales)
            {
                var product = await _context.Products.FindAsync(sale.ProductId);
                if (product != null)
                    products.Add((product, sale.TotalSold));
            }

            return products;
        }


        public async Task<List<(Product, int)>> PredictStockNeedsByWarehouseAsync(int warehouseId, int months)
        {
            var today = DateTime.Today;
            var pastData = await _context.ExitDetails
                .Include(ed => ed.InventoryExit)
                .Where(ed => ed.InventoryExit.WarehouseId == warehouseId && ed.InventoryExit.ExitDate >= today.AddMonths(-months))
                .GroupBy(ed => new { ed.ProductId, Month = ed.InventoryExit.ExitDate.Month })
                .Select(g => new
                {
                    g.Key.ProductId,
                    g.Key.Month,
                    TotalSold = g.Sum(ed => ed.Quantity)
                })
                .ToListAsync();

            var predictions = new List<(Product, int)>();

            var productGroups = pastData
                .GroupBy(p => p.ProductId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var productId in productGroups.Keys)
            {
                var sales = productGroups[productId]
                    .OrderBy(s => s.Month)
                    .Select(s => s.TotalSold)
                    .ToList();

                if (sales.Count >= 3)
                {
                    var lastThreeMonths = sales.Skip(Math.Max(0, sales.Count - 3)).ToList();
                    var predictedDemand = (int)Math.Round(lastThreeMonths.Average());

                    var product = await _context.Products.FindAsync(productId);
                    if (product != null)
                    {
                        predictions.Add((product, predictedDemand));
                    }
                }
            }

            return predictions;
        }



    }
}
