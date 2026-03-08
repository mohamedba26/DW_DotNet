using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Services.VwTotalSalesByYearService;
using System.Threading.Tasks;

namespace SalesDW.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "1")]
public class VwTotalSalesByYearController : ControllerBase
{
    private readonly IVwTotalSalesByYearService _service;

    public VwTotalSalesByYearController(IVwTotalSalesByYearService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int top = 50, [FromQuery] string order = "desc")
    {
        var items = await _service.GetAllAsync(top: top, order: order);
        return Ok(items);
    }
}
