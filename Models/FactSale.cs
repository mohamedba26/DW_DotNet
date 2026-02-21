using System;
using System.Collections.Generic;

namespace SalesDW.API.Models;

public partial class FactSale
{
    public int SalesKey { get; set; }

    public int? ProductKey { get; set; }

    public int? CustomerKey { get; set; }

    public int? TerritoryKey { get; set; }

    public int? OrderDateKey { get; set; }

    public int? OrderQty { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? LineTotal { get; set; }

    public virtual DimCustomer? CustomerKeyNavigation { get; set; }

    public virtual DimDate? OrderDateKeyNavigation { get; set; }

    public virtual DimProduct? ProductKeyNavigation { get; set; }

    public virtual DimTerritory? TerritoryKeyNavigation { get; set; }
}
