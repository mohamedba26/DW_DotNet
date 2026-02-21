using System;
using System.Collections.Generic;

namespace SalesDW.API.Models;

public partial class DimVendor
{
    public int VendorKey { get; set; }

    public int? BusinessEntityId { get; set; }

    public string? VendorName { get; set; }

    public string? AccountNumber { get; set; }

    public int? CreditRating { get; set; }

    public bool? ActiveFlag { get; set; }

    public virtual ICollection<FactPurchasing> FactPurchasings { get; set; } = new List<FactPurchasing>();
}
