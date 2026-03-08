namespace SalesDW.API.Models.ProductioDB;

public class CommandLineDto
{
    public int CommandLineId { get; set; }
    public int CommandId { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
