namespace SalesDW.API.Models;

public class AuthProduct
{
    public int Id { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal? StandardCost { get; set; }
    public decimal? ListPrice { get; set; }
    public string? Subcategory { get; set; }
    public string? Category { get; set; }
}