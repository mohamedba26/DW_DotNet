namespace SalesDW.API.Models;

public class UpdateUserDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? Role { get; set; }
}
