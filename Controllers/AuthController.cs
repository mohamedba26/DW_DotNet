using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesDW.API.Services.AuthService;
using SalesDW.API.Models;
using System.Security.Claims;

namespace SalesDW.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var user = await _auth.RegisterAsync(req.Email, req.Password);
        if (user == null) return BadRequest("User already exists");
        return Ok(new { user.Id, user.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var result = await _auth.LoginAsync(req.Email, req.Password);
        if (result == null) return Unauthorized();
        return Ok(new { token = result.Token, role = result.Role });
    }

    [HttpPut("reset-password")]
    [Authorize]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        // Use authenticated user's email to prevent changing other users' passwords
        var email = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(email)) return Unauthorized();

        var ok = await _auth.ResetPasswordAsync(email, dto.OldPassword, dto.NewPassword);
        if (!ok) return BadRequest("Invalid credentials or reset failed");
        return Ok();
    }
}

public class RegisterRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
