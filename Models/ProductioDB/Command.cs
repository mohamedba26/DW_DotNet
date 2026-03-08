namespace SalesDW.API.Models.ProductioDB;

public class Command
{
    public int CommandId { get; set; }
    public int UserId { get; set; }
    public int Approved { get; set; }

    public virtual ICollection<CommandLine> CommandLines { get; set; } = new List<CommandLine>();
}
