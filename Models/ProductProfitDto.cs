namespace SalesDW.API.Models;

public class ProductProfitDto
{
    public int ProductKey { get; set; }
    public string? ProductName { get; set; }

    public decimal TotalSalesAmount { get; set; }
    public decimal TotalPurchaseAmount { get; set; }

    public int TotalQtySold { get; set; }
    public int TotalQtyPurchased { get; set; }

    public decimal Profit { get; set; }
}
