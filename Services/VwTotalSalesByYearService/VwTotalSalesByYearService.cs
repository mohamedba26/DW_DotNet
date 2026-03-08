using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwTotalSalesByYearService;

public class VwTotalSalesByYearService : IVwTotalSalesByYearService
{
    private readonly DwSalesPurchasingContext _context;

    public VwTotalSalesByYearService(DwSalesPurchasingContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<VwTotalSalesByYear>> GetAllAsync(int top = 50, string order = "desc")
    {
        if (top < 1) top = 50;
        order = (order ?? "desc").ToLowerInvariant();

        var query = _context.VwTotalSalesByYears.AsNoTracking();
        var total = await query.LongCountAsync();

        var itemsQuery = order == "asc"
            ? query.OrderBy(v => v.YearNumber)
            : query.OrderByDescending(v => v.YearNumber);

        var items = await itemsQuery
            .Take(top)
            .ToListAsync();

        return new PagedResult<VwTotalSalesByYear>
        {
            Page = 1,
            PageSize = top,
            TotalCount = total,
            Items = items
        };
    }
}
