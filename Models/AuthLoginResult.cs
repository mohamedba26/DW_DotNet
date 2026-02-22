namespace SalesDW.API.Models;

public class AuthLoginResult
{
    public string Token { get; set; } = null!;
    public int Role { get; set; }
}