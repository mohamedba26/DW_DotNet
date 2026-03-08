using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Services.CommandService;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using SalesDW.API.Models.ProductioDB;

namespace SalesDW.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommandController : ControllerBase
{
    private readonly ICommandService _service;

    public CommandController(ICommandService service)
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

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

        var cmd = new Command
        {
            UserId = int.Parse(userIdClaim),
            Approved = 0
        };

        var created = await _service.CreateAsync(cmd);
        return CreatedAtAction(nameof(GetById), new { id = created.CommandId }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> Update(int id, [FromBody] Command cmd)
    {
        var updated = await _service.UpdateAsync(id, cmd);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
