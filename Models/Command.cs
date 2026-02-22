using System;
using System.Collections.Generic;

namespace SalesDW.API.Models;

public class Command
{
    public int CommandId { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }

    public virtual ICollection<CommandLine> CommandLines { get; set; } = new List<CommandLine>();
}
