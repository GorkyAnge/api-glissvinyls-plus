using glissvinyls_plus.Context;
using glissvinyls_plus.Interfaces;
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
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        // GET: api/Warehouses
        [HttpGet]
        public async Task<IActionResult> GetWarehouses()
        {
            try
            {
                var warehouses = await _warehouseService.GetWarehousesAsync();
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
                var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);
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
                await _warehouseService.CreateWarehouseAsync(request);
                return Ok(new { Success = true, Message = "Warehouse created successfully" });
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
            try
            {
                await _warehouseService.UpdateWarehouseAsync(id, request);
                return Ok(new { Success = true, Message = "Warehouse updated successfully" });
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
            try
            {
                await _warehouseService.DeleteWarehouseAsync(id);
                return NoContent(); // Código 204
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
