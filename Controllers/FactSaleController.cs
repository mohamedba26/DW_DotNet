using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Services.FactSaleService;
using System.Threading.Tasks;

namespace SalesDW.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1")]
    public class FactSaleController : ControllerBase
    {
        private readonly IFactSaleService _service;

        public FactSaleController(IFactSaleService service)
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
