namespace SalesDW.API.Models;

public class DimProductDto
{
    public int ProductKey { get; set; }
    public string? ProductName { get; set; }
    public string? Subcategory { get; set; }
    public string? Category { get; set; }
    public decimal? ListPrice { get; set; }
}
