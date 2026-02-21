using System;
using System.Collections.Generic;

namespace SalesDW.API.Models;

public partial class DimCustomer
{
    public int CustomerKey { get; set; }

    public int? CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FullName { get; set; }

    public string? EmailAddress { get; set; }

    public string? CustomerType { get; set; }

    public virtual ICollection<FactSale> FactSales { get; set; } = new List<FactSale>();
}
