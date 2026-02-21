using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Services.StatisticsService;
using System.Threading.Tasks;

namespace SalesDW.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _service;

        public StatisticsController(IStatisticsService service)
        {
            _service = service;
        }

        [HttpGet("purchasing-by-vendor")]
        public async Task<IActionResult> PurchasingByVendor([FromQuery] string sort = "desc", [FromQuery] string? category = null)
        {
            var data = await _service.GetPurchasingByVendorAsync(sort, category);
            return Ok(data);
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> TopProducts([FromQuery] int top = 10, [FromQuery] string sort = "desc")
        {
            var data = await _service.GetTopProductsAsync(top, sort);
            return Ok(data);
        }

        [HttpGet("sales-by-territory")]
        public async Task<IActionResult> SalesByTerritory([FromQuery] string sort = "desc", [FromQuery] string? category = null)
        {
            var data = await _service.GetSalesByTerritoryAsync(sort, category);
            return Ok(data);
        }

        [HttpGet("sales-by-year")]
        public async Task<IActionResult> SalesByYear([FromQuery] string sort = "desc", [FromQuery] string? category = null)
        {
            var data = await _service.GetSalesByYearAsync(sort, category);
            return Ok(data);
        }

        [HttpGet("sales-by-vendor")]
        public async Task<IActionResult> SalesByVendor([FromQuery] string sort = "desc", [FromQuery] int top = 0, [FromQuery] string? category = null)
        {
            var data = await _service.GetSalesByVendorAsync(sort, top, category);
            return Ok(data);
        }

        [HttpGet("products-by-profit")]
        public async Task<IActionResult> ProductsByProfit([FromQuery] int top = 10, [FromQuery] string? category = null, [FromQuery] string sort = "desc")
        {
            var data = await _service.GetProductsByProfitAsync(top, category, sort);
            return Ok(data);
        }

        [HttpGet("time-series")]
        public async Task<IActionResult> TimeSeries([FromQuery] string metric = "sales", [FromQuery] string period = "month", [FromQuery] int months = 12, [FromQuery] string? category = null)
        {
            var data = await _service.GetTimeSeriesAsync(metric, period, months, category);
            return Ok(data);
        }
    }
}
