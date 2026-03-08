using System.Collections.Generic;

namespace SalesDW.API.Models.ProductioDB;

public class CommandDto
{
    public int CommandId { get; set; }
    public int UserId { get; set; }
    public string? Email { get; set; }
    public int Approved { get; set; }
    public IEnumerable<CommandLine> CommandLines { get; set; } = new List<CommandLine>();
}
