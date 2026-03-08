using System;
using System.Collections.Generic;

namespace SalesDW.API.Models.DW.Views;

public partial class VwTotalSalesByYear
{
    public int? YearNumber { get; set; }

    public decimal? TotalSales { get; set; }
}
