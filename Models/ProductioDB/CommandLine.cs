using System.Text.Json.Serialization;

namespace SalesDW.API.Models.ProductioDB;

public class CommandLine
{
    public int CommandLineId { get; set; }
    public int CommandId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    [JsonIgnore]
    public virtual Command? Command { get; set; }
}
