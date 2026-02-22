namespace SalesDW.API.Models;

public class CommandLine
{
    public int CommandLineId { get; set; }
    public int CommandId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public virtual Command? Command { get; set; }
}
