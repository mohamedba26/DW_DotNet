using System;
using System.Collections.Generic;

namespace SalesDW.API.Models.DW.Views;

public partial class VwSalesByTerritory
{
    public string? TerritoryName { get; set; }

    public decimal? TotalSales { get; set; }
}
