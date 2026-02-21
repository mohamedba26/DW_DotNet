using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Services.FactPurchasingService;
using System.Threading.Tasks;

namespace SalesDW.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1")]
    public class FactPurchasingController : ControllerBase
    {
        private readonly IFactPurchasingService _service;

        public FactPurchasingController(IFactPurchasingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var items = await _service.GetAllAsync(page, pageSize);
            return Ok(items);
        }
    }
}
