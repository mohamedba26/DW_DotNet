using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwProductProfitService;

public class VwProductProfitService : IVwProductProfitService
{
    private readonly DwSalesPurchasingContext _context;

    public VwProductProfitService(DwSalesPurchasingContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<VwProductProfit>> GetAllAsync(int top = 50, string order = "desc", string? category = null)
    {
        if (top < 1) top = 50;
        order = (order ?? "desc").ToLowerInvariant();

        var query = _context.VwProductProfits.AsNoTracking();

        if (!string.IsNullOrEmpty(category) && category.ToLowerInvariant() != "all")
        {
            query = query.Where(v => v.Category == category);
        }

        var total = await query.LongCountAsync();

        var itemsQuery = order == "asc"
            ? query.OrderBy(v => v.Profit)
            : query.OrderByDescending(v => v.Profit);

        var items = await itemsQuery
            .Take(top)
            .ToListAsync();

        return new PagedResult<VwProductProfit>
        {
            Page = 1,
            PageSize = top,
            TotalCount = total,
            Items = items
        };
    }

    public async Task<VwProductProfit?> GetByProductKeyAsync(int productKey)
    {
        return await _context.VwProductProfits.AsNoTracking().FirstOrDefaultAsync(v => v.ProductKey == productKey);
    }
}
