using System;
using System.Collections.Generic;

namespace SalesDW.API.Models;

public partial class DimProduct
{
    public int ProductKey { get; set; }

    public int? ProductId { get; set; }

    public decimal? StandardCost { get; set; }

    public decimal? ListPrice { get; set; }

    public string? ProductName { get; set; }

    public string? Subcategory { get; set; }

    public string? Category { get; set; }

    public virtual ICollection<FactPurchasing> FactPurchasings { get; set; } = new List<FactPurchasing>();

    public virtual ICollection<FactSale> FactSales { get; set; } = new List<FactSale>();
}
