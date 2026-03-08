using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwPurchasingBaseService;

public class VwPurchasingBaseService : IVwPurchasingBaseService
{
    private readonly DwSalesPurchasingContext _context;

    public VwPurchasingBaseService(DwSalesPurchasingContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<VwPurchasingBase>> GetAllAsync(int top = 50, string order = "desc", string? category = null)
    {
        if (top < 1) top = 50;
        order = (order ?? "desc").ToLowerInvariant();

        var query = _context.VwPurchasingBases.AsNoTracking();

        if (!string.IsNullOrEmpty(category) && category.ToLowerInvariant() != "all")
        {
            query = query.Where(v => v.Category == category);
        }

        var total = await query.LongCountAsync();

        var itemsQuery = order == "asc"
            ? query.OrderBy(v => v.LineTotal)
            : query.OrderByDescending(v => v.LineTotal);

        var items = await itemsQuery
            .Take(top)
            .ToListAsync();

        return new PagedResult<VwPurchasingBase>
        {
            Page = 1,
            PageSize = top,
            TotalCount = total,
            Items = items
        };
    }
}
