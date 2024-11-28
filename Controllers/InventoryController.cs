using glissvinyls_plus.Models.RequestModels;
using glissvinyls_plus.Services;
using Microsoft.AspNetCore.Mvc;

namespace glissvinyls_plus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly AcquisitionService _acquisitionService;

        public InventoryController(AcquisitionService acquisitionService)
        {
            _acquisitionService = acquisitionService;
        }

        [HttpPost("acquire")]
        public async Task<IActionResult> AcquireProducts([FromBody] AcquisitionRequest request)
        {
            try
            {
                var result = await _acquisitionService.AcquireProductsAsync(request);
                return Ok(new { Success = result, Message = "Products acquired successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
