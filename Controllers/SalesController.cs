using glissvinyls_plus.Models.RequestModels;
using glissvinyls_plus.Services;
using Microsoft.AspNetCore.Mvc;

namespace glissvinyls_plus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly SalesService _salesService;

        public SalesController(SalesService salesService)
        {
            _salesService = salesService;
        }

        /// <summary>
        /// Endpoint para registrar una venta
        /// </summary>
        /// <param name="request">Detalle de la venta</param>
        /// <returns>Resultado del registro</returns>
        [HttpPost("register-sale")]
        public async Task<IActionResult> RegisterSale([FromBody] SaleRequest request)
        {
            if (request == null || request.Products == null || !request.Products.Any())
            {
                return BadRequest(new { Message = "Invalid sale request. Please provide valid data." });
            }

            try
            {
                var success = await _salesService.RegisterSaleAsync(request);
                if (success)
                {
                    return Ok(new { Message = "Sale registered successfully." });
                }
                return BadRequest(new { Message = "Failed to register sale." });
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
