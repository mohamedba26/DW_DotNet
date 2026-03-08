using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Services.VwSalesByTerritoryService;
using System.Threading.Tasks;

namespace SalesDW.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "1")]
public class VwSalesByTerritoryController : ControllerBase
{
    private readonly IVwSalesByTerritoryService _service;

    public VwSalesByTerritoryController(IVwSalesByTerritoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int top = 50, [FromQuery] string order = "desc", [FromQuery] string? territory = null)
    {
        var items = await _service.GetAllAsync(top: top, order: order, territory: territory);
        return Ok(items);
    }
}
