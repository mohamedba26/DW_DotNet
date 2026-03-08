using System;
using System.Collections.Generic;

namespace SalesDW.API.Models.DW.Views;

public partial class VwProductProfit
{
    public int ProductKey { get; set; }

    public string? ProductName { get; set; }

    public string? Category { get; set; }

    public decimal TotalSalesAmount { get; set; }

    public int TotalQtySold { get; set; }

    public decimal TotalPurchaseAmount { get; set; }

    public int TotalQtyPurchased { get; set; }

    public decimal? Profit { get; set; }
}
