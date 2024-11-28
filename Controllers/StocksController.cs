using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using glissvinyls_plus.Context;
using glissvinyls_plus.Models;
using Microsoft.AspNetCore.Authorization;

namespace glissvinyls_plus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StocksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Stocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
        {
            return await _context.Stocks.ToListAsync();
        }

        // GET: api/Stocks/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Stock>> GetStock(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return stock;
        }

        // PUT: api/Stocks/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutStock(int id, Stock stock)
        {
            if (id != stock.StockId)
            {
                return BadRequest();
            }

            _context.Entry(stock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Stocks
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Stock>> PostStock(Stock stock)
        {
            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStock", new { id = stock.StockId }, stock);
        }

        // DELETE: api/Stocks/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// GET: api/Stocks/warehouse/{warehouseId}
        [HttpGet("warehouse/{warehouseId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetStockByWarehouse(int warehouseId)
        {
            // Consultar el stock disponible para el almacén especificado
            var stock = await _context.Stocks
                .Include(s => s.Product) // Incluye la información del producto
                .Where(s => s.WarehouseId == warehouseId) // Filtra por almacén
                .Select(s => new
                {
                    StockId = s.StockId, // Incluir StockId
                    ProductId = s.ProductId,
                    ProductName = s.Product.Name,
                    ProductDescription = s.Product.Description,
                    AvailableQuantity = s.AvailableQuantity
                })
                .ToListAsync();

            // Verificar si no hay stock para el almacén
            if (!stock.Any())
            {
                return NotFound(new { Message = $"No stock found for warehouse with ID {warehouseId}" });
            }

            return Ok(stock);
        }


        [HttpPut("{id}/quantity")]
        public async Task<IActionResult> UpdateStockQuantity(int id, [FromBody] int newQuantity)
        {
            // Buscar el stock por ID
            var stock = await _context.Stocks
                .Include(s => s.Product) // Incluir el producto relacionado para registrar el movimiento
                .Include(s => s.Warehouse) // Incluir el almacén relacionado para registrar el movimiento
                .FirstOrDefaultAsync(s => s.StockId == id);

            if (stock == null)
            {
                return NotFound(new { Message = $"Stock with ID {id} not found." });
            }

            // Calcular la diferencia entre la cantidad actual y la nueva
            var quantityDifference = newQuantity - stock.AvailableQuantity;

            // Actualizar la cantidad disponible en el stock
            stock.AvailableQuantity = newQuantity;

            try
            {
                // Guardar los cambios en el stock
                _context.Stocks.Update(stock);

                // Registrar el movimiento solo si hay un incremento
                if (quantityDifference > 0)
                {
                    var movement = new MovementHistory
                    {
                        ProductId = stock.ProductId,
                        MovementDate = DateTime.Now,
                        MovementType = "Entry",
                        Quantity = quantityDifference,
                        WarehouseId = stock.WarehouseId,
                        Product = stock.Product,
                        Warehouse = stock.Warehouse
                    };

                    _context.MovementHistories.Add(movement);
                }

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Stock updated successfully.", MovementRegistered = quantityDifference > 0 });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(id))
                {
                    return NotFound(new { Message = $"Stock with ID {id} not found during save operation." });
                }
                else
                {
                    throw;
                }
            }
        }



        private bool StockExists(int id)
        {
            return _context.Stocks.Any(e => e.StockId == id);
        }
    }
}
