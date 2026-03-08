using System;
using System.Collections.Generic;

namespace SalesDW.API.Models.DW.Views;

public partial class VwPurchasingByVendor
{
    public string? VendorName { get; set; }

    public decimal? TotalPurchasing { get; set; }
}
