using glissvinyls_plus.Context;
using glissvinyls_plus.Models;
using glissvinyls_plus.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace glissvinyls_plus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WarehousesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Warehouses
        [HttpGet]
        public async Task<IActionResult> GetWarehouses()
        {
            try
            {
                var warehouses = await _context.Warehouses
                    .Select(w => new
                    {
                        w.WarehouseId,
                        w.WarehouseName,
                        w.Address
                    })
                    .ToListAsync();

                return Ok(new { Success = true, Warehouses = warehouses });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        // GET: api/Warehouses/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWarehouseById(int id)
        {
            try
            {
                var warehouse = await _context.Warehouses
                    .Where(w => w.WarehouseId == id)
                    .Select(w => new
                    {
                        w.WarehouseId,
                        w.WarehouseName,
                        w.Address
                    })
                    .FirstOrDefaultAsync();

                if (warehouse == null)
                {
                    return NotFound(new { Success = false, Message = "Warehouse not found" });
                }

                return Ok(new { Success = true, Warehouse = warehouse });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        // POST: api/Warehouses
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateWarehouse([FromBody] WarehouseRequest request)
        {
            try
            {
                var warehouse = new Warehouse
                {
                    WarehouseName = request.WarehouseName,
                    Address = request.Address
                };

                _context.Warehouses.Add(warehouse);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetWarehouseById), new { id = warehouse.WarehouseId }, new { Success = true, Warehouse = warehouse });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        // PUT: api/Warehouses/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateWarehouse(int id, [FromBody] WarehouseRequest request)
        {
            if (id <= 0)
            {
                return BadRequest(new { Success = false, Message = "Invalid Warehouse ID" });
            }

            try
            {
                var warehouse = await _context.Warehouses.FindAsync(id);

                if (warehouse == null)
                {
                    return NotFound(new { Success = false, Message = "Warehouse not found" });
                }

                warehouse.WarehouseName = request.WarehouseName;
                warehouse.Address = request.Address;

                _context.Entry(warehouse).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { Success = true, Message = "Warehouse updated successfully", Warehouse = warehouse });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Success = false, Message = "Concurrency error occurred while updating the warehouse" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        // DELETE: api/Warehouses/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Success = false, Message = "Invalid Warehouse ID" });
            }

            try
            {
                var warehouse = await _context.Warehouses.FindAsync(id);

                if (warehouse == null)
                {
                    return NotFound(new { Success = false, Message = "Warehouse not found" });
                }

                _context.Warehouses.Remove(warehouse);
                await _context.SaveChangesAsync();

                return NoContent(); // No devuelve contenido, solo código 204
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
