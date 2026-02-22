using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Models;
using SalesDW.API.Services.AuthProductService;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SalesDW.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthProductController : ControllerBase
{
    private readonly IAuthProductService _service;

    public AuthProductController(IAuthProductService service)
    {
        _service = service;
    }

    [HttpGet]
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
    [Authorize(Roles = "1")]
    public async Task<IActionResult> Create([FromForm] AuthProduct p, IFormFile? image)
    {
        if (image != null && image.Length > 0)
        {
            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            var bytes = ms.ToArray();
            var base64 = Convert.ToBase64String(bytes);
            p.Image = $"data:{image.ContentType};base64,{base64}";
        }

        var created = await _service.CreateAsync(p);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> Update(int id, [FromForm] AuthProduct p, IFormFile? image)
    {
        if (image != null && image.Length > 0)
        {
            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            var bytes = ms.ToArray();
            var base64 = Convert.ToBase64String(bytes);
            p.Image = $"data:{image.ContentType};base64,{base64}";
        }

        var updated = await _service.UpdateAsync(id, p);
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
