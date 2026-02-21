namespace SalesDW.API.Models;

public class FactPurchasingDto
{
    public int PurchasingKey { get; set; }
    public int? ProductKey { get; set; }
    public int? VendorKey { get; set; }
    public int? OrderDateKey { get; set; }
    public int? OrderQty { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? LineTotal { get; set; }

    public string? ProductName { get; set; }
    public string? VendorName { get; set; }

    public static FactPurchasingDto FromFactPurchasing(FactPurchasing model)
    {
        if (model == null) return null!;

        return new FactPurchasingDto
        {
            PurchasingKey = model.PurchasingKey,
            ProductKey = model.ProductKey,
            VendorKey = model.VendorKey,
            OrderDateKey = model.OrderDateKey,
            OrderQty = model.OrderQty,
            UnitCost = model.UnitCost,
            LineTotal = model.LineTotal,
            ProductName = model.ProductKeyNavigation?.ProductName,
            VendorName = model.VendorKeyNavigation?.VendorName
        };
    }

    public FactPurchasing ToFactPurchasing()
    {
        return new FactPurchasing
        {
            PurchasingKey = this.PurchasingKey,
            ProductKey = this.ProductKey,
            VendorKey = this.VendorKey,
            OrderDateKey = this.OrderDateKey,
            OrderQty = this.OrderQty,
            UnitCost = this.UnitCost,
            LineTotal = this.LineTotal
        };
    }
}
