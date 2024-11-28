using glissvinyls_plus.Models.DTO;
using glissvinyls_plus.Services;
using Microsoft.AspNetCore.Mvc;

namespace glissvinyls_plus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly RecommendationService _service;

        public RecommendationsController(RecommendationService service)
        {
            _service = service;
        }

        [HttpGet("top-selling-products")]
        public async Task<IActionResult> GetTopSellingProducts()
        {
            var result = await _service.GetTopSellingProductsAsync();
            return Ok(result.Select(r => new { r.Item1.Name, r.Item1.Description, TotalSold = r.Item2 }));
        }

        [HttpGet("predict-stock-needs")]
        public async Task<IActionResult> PredictStockNeeds([FromQuery] int months = 3)
        {
            try
            {
                if (months <= 0 || months > 24)
                {
                    return BadRequest("El parámetro 'months' debe estar entre 1 y 24.");
                }

                var predictions = await _service.PredictStockNeedsAsync(months);
                var predictionDtos = predictions.Select(p => new StockPredictionDto
                {
                    ProductName = p.Item1.Name,
                    Description = p.Item1.Description,
                    PredictedQuantity = p.Item2
                }).ToList();

                return Ok(predictionDtos);
            }
            catch (Exception ex)
            {
                // Considera utilizar logging aquí
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        [HttpGet("top-selling-products-by-warehouse/{warehouseId}")]
        public async Task<IActionResult> GetTopSellingProductsByWarehouse(int warehouseId)
        {
            var result = await _service.GetTopSellingProductsByWarehouseAsync(warehouseId);
            return Ok(result.Select(r => new
            {
                r.Item1.Name,
                r.Item1.Description,
                TotalSold = r.Item2
            }));
        }

        [HttpGet("predict-stock-needs-by-warehouse/{warehouseId}")]
        public async Task<IActionResult> PredictStockNeedsByWarehouse(int warehouseId, [FromQuery] int months = 3)
        {
            try
            {
                if (months <= 0 || months > 24)
                {
                    return BadRequest("El parámetro 'months' debe estar entre 1 y 24.");
                }

                var predictions = await _service.PredictStockNeedsByWarehouseAsync(warehouseId, months);
                var predictionDtos = predictions.Select(p => new StockPredictionDto
                {
                    ProductName = p.Item1.Name,
                    Description = p.Item1.Description,
                    PredictedQuantity = p.Item2
                }).ToList();

                return Ok(predictionDtos);
            }
            catch (Exception ex)
            {
                // Considera utilizar logging aquí
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }



    }
}
