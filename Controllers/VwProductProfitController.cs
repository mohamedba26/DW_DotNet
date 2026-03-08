using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Services.VwProductProfitService;
using System.Threading.Tasks;

namespace SalesDW.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "1")]
public class VwProductProfitController : ControllerBase
{
    private readonly IVwProductProfitService _service;

    public VwProductProfitController(IVwProductProfitService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int top = 50, [FromQuery] string order = "desc", [FromQuery] string? category = null)
    {
        var items = await _service.GetAllAsync(top: top, order: order, category: category);
        return Ok(items);
    }

    [HttpGet("product/{productKey:int}")]
    public async Task<IActionResult> GetByProductKey(int productKey)
    {
        var item = await _service.GetByProductKeyAsync(productKey);
        if (item == null) return NotFound();
        return Ok(item);
    }
}
