using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwTopProductService;

public class VwTopProductService : IVwTopProductService
{
    private readonly DwSalesPurchasingContext _context;

    public VwTopProductService(DwSalesPurchasingContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<VwTopProduct>> GetAllAsync(int top = 50, string order = "desc", string? category = null)
    {
        if (top < 1) top = 50;
        order = (order ?? "desc").ToLowerInvariant();

        var query = _context.VwTopProducts.AsNoTracking();

        // view doesn't expose category; if category filtering is needed, create a specialized view or stored proc
        var total = await query.LongCountAsync();

        var itemsQuery = order == "asc"
            ? query.OrderBy(v => v.Revenue)
            : query.OrderByDescending(v => v.Revenue);

        var items = await itemsQuery
            .Take(top)
            .ToListAsync();

        return new PagedResult<VwTopProduct>
        {
            Page = 1,
            PageSize = top,
            TotalCount = total,
            Items = items
        };
    }
}
