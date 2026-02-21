using System;
using System.Collections.Generic;

namespace SalesDW.API.Models;

public partial class DimTerritory
{
    public int TerritoryKey { get; set; }

    public int? TerritoryId { get; set; }

    public string? TerritoryName { get; set; }

    public string? CountryRegionCode { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<FactSale> FactSales { get; set; } = new List<FactSale>();
}
