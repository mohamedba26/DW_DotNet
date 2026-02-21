namespace SalesDW.API.Models;

public class PurchasingByVendorDto
{
    public int? VendorKey { get; set; }
    public string? VendorName { get; set; }
    public string? Category { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalQty { get; set; }
}

public class TopProductDto
{
    public int? ProductKey { get; set; }
    public string? ProductName { get; set; }
    public string? Category { get; set; }
    public decimal TotalSalesAmount { get; set; }
    public int TotalQty { get; set; }
}

public class SalesByTerritoryDto
{
    public int? TerritoryKey { get; set; }
    public string? TerritoryName { get; set; }
    public string? Category { get; set; }
    public decimal TotalSalesAmount { get; set; }
    public int TotalQty { get; set; }
}

public class SalesByYearDto
{
    public int? Year { get; set; }
    public decimal TotalSalesAmount { get; set; }
    public int TotalQty { get; set; }
}

public class SalesByVendorDto
{
    public int? VendorKey { get; set; }
    public string? VendorName { get; set; }
    public string? Category { get; set; }
    public decimal TotalSalesAmount { get; set; }
    public int TotalQty { get; set; }
}
