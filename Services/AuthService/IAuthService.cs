using SalesDW.API.Models;

namespace SalesDW.API.Services.AuthService;

public interface IAuthService
{
    Task<AuthUser?> RegisterAsync(string email, string password);
    Task<string?> LoginAsync(string email, string password);
    Task<bool> ResetPasswordAsync(string email, string oldPassword, string newPassword);
}
