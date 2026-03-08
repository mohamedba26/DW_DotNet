using System;
using System.Collections.Generic;

namespace SalesDW.API.Models.DW.Views;

public partial class VwSalesBase
{
    public int? ProductKey { get; set; }

    public string? ProductName { get; set; }

    public string? Category { get; set; }

    public int? TerritoryKey { get; set; }

    public string? TerritoryName { get; set; }

    public int? OrderDateKey { get; set; }

    public int? YearNumber { get; set; }

    public int? MonthNumber { get; set; }

    public int? DayNumber { get; set; }

    public decimal? LineTotal { get; set; }

    public int? OrderQty { get; set; }
}
