using glissvinyls_plus.Context;
using glissvinyls_plus.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace glissvinyls_plus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovementHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovementHistoryController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Mostrar el historial de movimientos de Warehouses específicos
        [HttpGet("ByWarehouse/{warehouseId}")]
        public async Task<ActionResult<IEnumerable<MovementHistoryDto>>> GetMovementHistoryByWarehouse(int warehouseId)
        {
            var movements = await _context.MovementHistories
                .Include(m => m.Product)
                .Include(m => m.Warehouse)
                .Where(m => m.WarehouseId == warehouseId)
                .Select(m => new MovementHistoryDto
                {
                    HistoryId = m.HistoryId,
                    ProductId = m.ProductId,
                    ProductName = m.Product.Name,
                    MovementDate = m.MovementDate,
                    MovementType = m.MovementType,
                    Quantity = m.Quantity,
                    WarehouseId = m.WarehouseId,
                    WarehouseName = m.Warehouse.WarehouseName
                })
                .ToListAsync();

            return Ok(movements);
        }

        // 2. Mostrar el historial completo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovementHistoryDto>>> GetAllMovementHistory()
        {
            var movements = await _context.MovementHistories
                .Include(m => m.Product)
                .Include(m => m.Warehouse)
                .Select(m => new MovementHistoryDto
                {
                    HistoryId = m.HistoryId,
                    ProductId = m.ProductId,
                    ProductName = m.Product.Name,
                    MovementDate = m.MovementDate,
                    MovementType = m.MovementType,
                    Quantity = m.Quantity,
                    WarehouseId = m.WarehouseId,
                    WarehouseName = m.Warehouse.WarehouseName
                })
                .ToListAsync();

            return Ok(movements);
        }

        // 3. Mostrar el historial de Exit
        [HttpGet("Exit")]
        public async Task<ActionResult<IEnumerable<MovementHistoryDto>>> GetExitHistory()
        {
            var movements = await _context.MovementHistories
                .Include(m => m.Product)
                .Include(m => m.Warehouse)
                .Where(m => m.MovementType.ToLower() == "exit") // Cambiado para evitar problemas de traducción
                .Select(m => new MovementHistoryDto
                {
                    HistoryId = m.HistoryId,
                    ProductId = m.ProductId,
                    ProductName = m.Product.Name,
                    MovementDate = m.MovementDate,
                    MovementType = m.MovementType,
                    Quantity = m.Quantity,
                    WarehouseId = m.WarehouseId,
                    WarehouseName = m.Warehouse.WarehouseName
                })
                .ToListAsync();

            return Ok(movements);
        }


        // 4. Mostrar el historial de Entry
        [HttpGet("Entry")]
        public async Task<ActionResult<IEnumerable<MovementHistoryDto>>> GetEntryHistory()
        {
            var movements = await _context.MovementHistories
                .Include(m => m.Product)
                .Include(m => m.Warehouse)
                .Where(m => m.MovementType.ToLower() == "entry") // Cambiar a ToLower() para evitar errores de traducción
                .Select(m => new MovementHistoryDto
                {
                    HistoryId = m.HistoryId,
                    ProductId = m.ProductId,
                    ProductName = m.Product.Name,
                    MovementDate = m.MovementDate,
                    MovementType = m.MovementType,
                    Quantity = m.Quantity,
                    WarehouseId = m.WarehouseId,
                    WarehouseName = m.Warehouse.WarehouseName
                })
                .ToListAsync();

            return Ok(movements);
        }

        // 5. Mostrar el historial de entradas por Warehouse
        [HttpGet("ByWarehouse/{warehouseId}/Entry")]
        public async Task<ActionResult<IEnumerable<MovementHistoryDto>>> GetEntryHistoryByWarehouse(int warehouseId)
        {
            var movements = await _context.MovementHistories
                .Include(m => m.Product)
                .Include(m => m.Warehouse)
                .Where(m => m.WarehouseId == warehouseId && m.MovementType.ToLower() == "entry") // Filtrar por WarehouseId y tipo Entry
                .Select(m => new MovementHistoryDto
                {
                    HistoryId = m.HistoryId,
                    ProductId = m.ProductId,
                    ProductName = m.Product.Name,
                    MovementDate = m.MovementDate,
                    MovementType = m.MovementType,
                    Quantity = m.Quantity,
                    WarehouseId = m.WarehouseId,
                    WarehouseName = m.Warehouse.WarehouseName
                })
                .ToListAsync();

            return Ok(movements);
        }

        // 6. Mostrar el historial de salidas por Warehouse
        [HttpGet("ByWarehouse/{warehouseId}/Exit")]
        public async Task<ActionResult<IEnumerable<MovementHistoryDto>>> GetExitHistoryByWarehouse(int warehouseId)
        {
            var movements = await _context.MovementHistories
                .Include(m => m.Product)
                .Include(m => m.Warehouse)
                .Where(m => m.WarehouseId == warehouseId && m.MovementType.ToLower() == "exit") // Filtrar por WarehouseId y tipo Exit
                .Select(m => new MovementHistoryDto
                {
                    HistoryId = m.HistoryId,
                    ProductId = m.ProductId,
                    ProductName = m.Product.Name,
                    MovementDate = m.MovementDate,
                    MovementType = m.MovementType,
                    Quantity = m.Quantity,
                    WarehouseId = m.WarehouseId,
                    WarehouseName = m.Warehouse.WarehouseName
                })
                .ToListAsync();

            return Ok(movements);
        }

        // 7. Obtener productos con mayor rotación en un rango de fechas
        [HttpGet("TopRotatedProducts")]
        public async Task<ActionResult<IEnumerable<TopRotatedProductDto>>> GetTopRotatedProducts(DateTime startDate, DateTime endDate)
        {
            var topRotatedProducts = await _context.MovementHistories
                .Where(m => m.MovementDate >= startDate && m.MovementDate <= endDate)
                .GroupBy(m => new { m.ProductId, m.Product.Name })
                .Select(g => new TopRotatedProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    TotalMovements = g.Count(),
                    TotalQuantity = g.Sum(m => m.Quantity),
                    EntryMovements = g.Count(m => m.MovementType.ToLower() == "entry"),
                    ExitMovements = g.Count(m => m.MovementType.ToLower() == "exit"),
                    EntryQuantity = g.Where(m => m.MovementType.ToLower() == "entry").Sum(m => m.Quantity),
                    ExitQuantity = g.Where(m => m.MovementType.ToLower() == "exit").Sum(m => m.Quantity)
                })
                .OrderByDescending(p => p.TotalMovements)
                .ToListAsync();

            return Ok(topRotatedProducts);
        }



    }

}
