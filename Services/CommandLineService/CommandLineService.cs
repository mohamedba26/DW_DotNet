using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SalesDW.API.Models.ProductioDB;

namespace SalesDW.API.Services.CommandLineService;

public class CommandLineService : ICommandLineService
{
    private readonly AuthDbContext _context;

    public CommandLineService(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommandLineDto>> GetAllAsync()
    {
        var lines = await _context.CommandLines.AsNoTracking().ToListAsync();
        var productIds = lines.Select(l => l.ProductId).Distinct().ToList();
        var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id, p => p);

        return lines.Select(l => new CommandLineDto
        {
            CommandLineId = l.CommandLineId,
            CommandId = l.CommandId,
            ProductId = l.ProductId,
            ProductName = products.TryGetValue(l.ProductId, out var prod) ? prod.ProductName : null,
            Quantity = l.Quantity,
            TotalPrice = (products.TryGetValue(l.ProductId, out prod) ? (prod.ListPrice ?? 0) : 0) * l.Quantity
        }).ToList();
    }

    public async Task<CommandLineDto?> GetByIdAsync(int id)
    {
        var line = await _context.CommandLines.AsNoTracking().FirstOrDefaultAsync(c => c.CommandLineId == id);
        if (line == null) return null;

        var prod = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == line.ProductId);

        return new CommandLineDto
        {
            CommandLineId = line.CommandLineId,
            CommandId = line.CommandId,
            ProductId = line.ProductId,
            ProductName = prod?.ProductName,
            Quantity = line.Quantity,
            TotalPrice = (prod?.ListPrice ?? 0) * line.Quantity
        };
    }

    public async Task<IEnumerable<CommandLineDto>> GetByCommandIdAsync(int commandId)
    {
        var lines = await _context.CommandLines.AsNoTracking().Where(cl => cl.CommandId == commandId).ToListAsync();
        var productIds = lines.Select(l => l.ProductId).Distinct().ToList();
        var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id, p => p);

        return lines.Select(l => new CommandLineDto
        {
            CommandLineId = l.CommandLineId,
            CommandId = l.CommandId,
            ProductId = l.ProductId,
            ProductName = products.TryGetValue(l.ProductId, out var prod) ? prod.ProductName : null,
            Quantity = l.Quantity,
            TotalPrice = (products.TryGetValue(l.ProductId, out prod) ? (prod.ListPrice ?? 0) : 0) * l.Quantity
        }).ToList();
    }

    public async Task<CommandLine> CreateAsync(CommandLine line)
    {
        // If a line with same CommandId and ProductId exists, update its quantity instead of creating a new one
        var existing = await _context.CommandLines
                                     .FirstOrDefaultAsync(cl => cl.CommandId == line.CommandId && cl.ProductId == line.ProductId);
        if (existing != null)
        {
            existing.Quantity += line.Quantity;
            await _context.SaveChangesAsync();
            return existing;
        }

        _context.CommandLines.Add(line);
        await _context.SaveChangesAsync();
        return line;
    }

    public async Task<CommandLine?> UpdateAsync(int id, CommandLine line)
    {
        var existing = await _context.CommandLines.FindAsync(id);
        if (existing == null) return null;

        existing.ProductId = line.ProductId;
        existing.Quantity = line.Quantity;
        existing.CommandId = line.CommandId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.CommandLines.FindAsync(id);
        if (existing == null) return false;

        _context.CommandLines.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
