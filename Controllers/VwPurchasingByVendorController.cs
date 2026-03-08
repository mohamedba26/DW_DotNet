using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Services.VwPurchasingByVendorService;
using System.Threading.Tasks;

namespace SalesDW.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "1")]
public class VwPurchasingByVendorController : ControllerBase
{
    private readonly IVwPurchasingByVendorService _service;

    public VwPurchasingByVendorController(IVwPurchasingByVendorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int top = 50, [FromQuery] string order = "desc", [FromQuery] string? category = null)
    {
        var items = await _service.GetAllAsync(top: top, order: order, category: category);
        return Ok(items);
    }
}
