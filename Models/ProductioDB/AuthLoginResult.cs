namespace SalesDW.API.Models.ProductioDB;

public class AuthLoginResult
{
    public string Token { get; set; } = null!;
    public int Role { get; set; }
}