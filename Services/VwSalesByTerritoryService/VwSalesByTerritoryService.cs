using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwSalesByTerritoryService;

public class VwSalesByTerritoryService : IVwSalesByTerritoryService
{
    private readonly DwSalesPurchasingContext _context;

    public VwSalesByTerritoryService(DwSalesPurchasingContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<VwSalesByTerritory>> GetAllAsync(int top = 50, string order = "desc", string? territory = null)
    {
        if (top < 1) top = 50;
        order = (order ?? "desc").ToLowerInvariant();

        var query = _context.VwSalesByTerritories.AsNoTracking();

        if (!string.IsNullOrEmpty(territory) && territory.ToLowerInvariant() != "all")
        {
            query = query.Where(v => v.TerritoryName == territory);
        }

        var total = await query.LongCountAsync();

        var itemsQuery = order == "asc"
            ? query.OrderBy(v => v.TotalSales)
            : query.OrderByDescending(v => v.TotalSales);

        var items = await itemsQuery
            .Take(top)
            .ToListAsync();

        return new PagedResult<VwSalesByTerritory>
        {
            Page = 1,
            PageSize = top,
            TotalCount = total,
            Items = items
        };
    }
}
