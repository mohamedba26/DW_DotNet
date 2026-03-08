using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Services.KpiService;
using System.Threading.Tasks;

namespace SalesDW.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "1")]
public class KpiController : ControllerBase
{
    private readonly IKpiService _service;

    public KpiController(IKpiService service)
    {
        _service = service;
    }

    [HttpGet("getKpis")]
    public async Task<IActionResult> GetKpis()
    {
        var result = await _service.GetKpisAsync();
        return Ok(result);
    }
}
