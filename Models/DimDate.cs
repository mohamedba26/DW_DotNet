using System;
using System.Collections.Generic;

namespace SalesDW.API.Models;

public partial class DimDate
{
    public int DateKey { get; set; }

    public DateOnly? FullDate { get; set; }

    public int? DayNumber { get; set; }

    public int? MonthNumber { get; set; }

    public string? MonthName { get; set; }

    public int? YearNumber { get; set; }

    public int? QuarterNumber { get; set; }

    public virtual ICollection<FactPurchasing> FactPurchasings { get; set; } = new List<FactPurchasing>();

    public virtual ICollection<FactSale> FactSales { get; set; } = new List<FactSale>();
}
