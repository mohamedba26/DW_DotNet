using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SalesDW.API.Data;
using SalesDW.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SalesDW.API.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly AuthDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AuthDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<AuthUser?> RegisterAsync(string email, string password)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == email);
        if (exists) return null;

        var salt = GenerateSalt();
        var hashed = HashPassword(password, salt);

        var user = new AuthUser
        {
            Email = email,
            PasswordHash = hashed,
            Salt = salt,
            Role = 2
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<AuthLoginResult?> LoginAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return null;

        if (!VerifyPassword(password, user.PasswordHash, user.Salt)) return null;

        // create JWT
        var key = _config["Jwt:Key"];
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"] ?? "60");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthLoginResult { Token = tokenString, Role = user.Role };
    }

    public async Task<bool> ResetPasswordAsync(string email, string oldPassword, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;

        if (!VerifyPassword(oldPassword, user.PasswordHash, user.Salt)) return false;

        var salt = GenerateSalt();
        user.Salt = salt;
        user.PasswordHash = HashPassword(newPassword, salt);

        await _context.SaveChangesAsync();
        return true;
    }

    private static string GenerateSalt(int size = 16)
    {
        var bytes = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string HashPassword(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hash);
    }

    private static bool VerifyPassword(string password, string hashed, string salt)
    {
        if (string.IsNullOrEmpty(salt))
        {
            // fallback legacy
            using var sha = SHA256.Create();
            var computedLegacy = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
            if (computedLegacy == hashed) return true;
        }

        var computed = HashPassword(password, salt);
        return hashed == computed;
    }
}
