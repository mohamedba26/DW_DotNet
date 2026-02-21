using System;
using System.Collections.Generic;

namespace SalesDW.API.Models;

public partial class FactPurchasing
{
    public int PurchasingKey { get; set; }

    public int? ProductKey { get; set; }

    public int? VendorKey { get; set; }

    public int? OrderDateKey { get; set; }

    public int? OrderQty { get; set; }

    public decimal? UnitCost { get; set; }

    public decimal? LineTotal { get; set; }

    public virtual DimDate? OrderDateKeyNavigation { get; set; }

    public virtual DimProduct? ProductKeyNavigation { get; set; }

    public virtual DimVendor? VendorKeyNavigation { get; set; }
}
