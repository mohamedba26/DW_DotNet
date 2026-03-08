using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwSalesBaseService;

public class VwSalesBaseService : IVwSalesBaseService
{
    private readonly DwSalesPurchasingContext _context;

    public VwSalesBaseService(DwSalesPurchasingContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<VwSalesBase>> GetAllAsync(int top = 50, string order = "desc", string? category = null)
    {
        if (top < 1) top = 50;
        order = (order ?? "desc").ToLowerInvariant();

        var query = _context.VwSalesBases.AsNoTracking();

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

        return new PagedResult<VwSalesBase>
        {
            Page = 1,
            PageSize = top,
            TotalCount = total,
            Items = items
        };
    }
}
