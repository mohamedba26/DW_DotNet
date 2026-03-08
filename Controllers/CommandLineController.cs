using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Services.CommandLineService;
using System.Threading.Tasks;
using System.Collections.Generic;
using SalesDW.API.Models.ProductioDB;

namespace SalesDW.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommandLineController : ControllerBase
{
    private readonly ICommandLineService _service;

    public CommandLineController(ICommandLineService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpGet("bypid/{commandId:int}")]
    public async Task<IActionResult> GetByCommandId(int commandId)
    {
        var items = await _service.GetByCommandIdAsync(commandId);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommandLine line)
    {
        var created = await _service.CreateAsync(line);
        return CreatedAtAction(nameof(GetById), new { id = created.CommandLineId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommandLine line)
    {
        var updated = await _service.UpdateAsync(id, line);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
