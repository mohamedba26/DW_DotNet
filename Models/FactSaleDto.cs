namespace SalesDW.API.Models;

public class FactSaleDto
{
    public int SalesKey { get; set; }
    public int? ProductKey { get; set; }
    public int? CustomerKey { get; set; }
    public int? TerritoryKey { get; set; }
    public int? OrderDateKey { get; set; }
    public int? OrderQty { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? LineTotal { get; set; }

    public string? ProductName { get; set; }
    public string? CustomerName { get; set; }
    public string? TerritoryName { get; set; }

    public static FactSaleDto FromFactSale(FactSale model)
    {
        if (model == null) return null!;

        return new FactSaleDto
        {
            SalesKey = model.SalesKey,
            ProductKey = model.ProductKey,
            CustomerKey = model.CustomerKey,
            TerritoryKey = model.TerritoryKey,
            OrderDateKey = model.OrderDateKey,
            OrderQty = model.OrderQty,
            UnitPrice = model.UnitPrice,
            LineTotal = model.LineTotal,
            ProductName = model.ProductKeyNavigation?.ProductName,
            CustomerName = model.CustomerKeyNavigation?.FullName,
            TerritoryName = model.TerritoryKeyNavigation?.TerritoryName
        };
    }

    public FactSale ToFactSale()
    {
        return new FactSale
        {
            SalesKey = this.SalesKey,
            ProductKey = this.ProductKey,
            CustomerKey = this.CustomerKey,
            TerritoryKey = this.TerritoryKey,
            OrderDateKey = this.OrderDateKey,
            OrderQty = this.OrderQty,
            UnitPrice = this.UnitPrice,
            LineTotal = this.LineTotal
        };
    }
}
