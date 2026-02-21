namespace SalesDW.API.Models;

public class DimCustomerDto
{
    public int CustomerKey { get; set; }
    public string? FullName { get; set; }
    public string? EmailAddress { get; set; }
    public string? CustomerType { get; set; }
}
