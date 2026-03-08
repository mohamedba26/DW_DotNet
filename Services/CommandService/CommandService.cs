using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using SalesDW.API.Models.ProductioDB;

namespace SalesDW.API.Services.CommandService;

public class CommandService : ICommandService
{
    private readonly AuthDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommandService(AuthDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<CommandDto>> GetAllAsync()
    {
        var query = await _context.Commands
            .AsNoTracking()
            .Include(c => c.CommandLines)
            .ToListAsync();

        var userIds = query.Select(c => c.UserId).Distinct().ToList();
        var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u.Email);

        return query.Select(c => new CommandDto
        {
            CommandId = c.CommandId,
            UserId = c.UserId,
            Email = users.TryGetValue(c.UserId, out var email) ? email : null,
            Approved = c.Approved,
            CommandLines = c.CommandLines
        });
    }

    public async Task<CommandDto?> GetByIdAsync(int id)
    {
        var cmd = await _context.Commands.Include(c => c.CommandLines).AsNoTracking().FirstOrDefaultAsync(c => c.CommandId == id);
        if (cmd == null) return null;

        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == cmd.UserId);

        return new CommandDto
        {
            CommandId = cmd.CommandId,
            UserId = cmd.UserId,
            Email = user?.Email,
            Approved = cmd.Approved,
            CommandLines = cmd.CommandLines
        };
    }

    public async Task<Command> CreateAsync(Command cmd)
    {
        // Determine user id: prefer token, fallback to provided cmd.UserId
        var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value
                        ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

        int userId = 0;
        if (!int.TryParse(userIdStr, out userId))
        {
            userId = cmd.UserId;
        }

        // If we have a valid userId, check for an existing pending command (Approved == 0)
        if (userId > 0)
        {
            var existing = await _context.Commands.Include(c => c.CommandLines)
                                                 .FirstOrDefaultAsync(c => c.UserId == userId && c.Approved == 0);
            if (existing != null)
            {
                return existing;
            }
        }

        // No existing pending command — set user and approved flag and create
        if (userId > 0)
        {
            cmd.UserId = userId;
        }
        cmd.Approved = 0;

        _context.Commands.Add(cmd);
        await _context.SaveChangesAsync();
        return cmd;
    }

    public async Task<Command?> UpdateAsync(int id, Command cmd)
    {
        var existing = await _context.Commands.FindAsync(id);
        if (existing == null) return null;

        existing.Approved = cmd.Approved;
        // do not allow changing UserId via update
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Commands.FindAsync(id);
        if (existing == null) return false;

        _context.Commands.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
