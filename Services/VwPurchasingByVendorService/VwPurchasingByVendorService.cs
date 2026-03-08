using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwPurchasingByVendorService;

public class VwPurchasingByVendorService : IVwPurchasingByVendorService
{
    private readonly DwSalesPurchasingContext _context;

    public VwPurchasingByVendorService(DwSalesPurchasingContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<VwPurchasingByVendor>> GetAllAsync(int top = 50, string order = "desc", string? category = null)
    {
        if (top < 1) top = 50;
        order = (order ?? "desc").ToLowerInvariant();

        var query = _context.VwPurchasingByVendors.AsNoTracking();

        if (!string.IsNullOrEmpty(category) && category.ToLowerInvariant() != "all")
        {
            query = query.Where(v => v.VendorName == v.VendorName); // placeholder, view doesn't contain category
        }

        var total = await query.LongCountAsync();

        var itemsQuery = order == "asc"
            ? query.OrderBy(v => v.TotalPurchasing)
            : query.OrderByDescending(v => v.TotalPurchasing);

        var items = await itemsQuery
            .Take(top)
            .ToListAsync();

        return new PagedResult<VwPurchasingByVendor>
        {
            Page = 1,
            PageSize = top,
            TotalCount = total,
            Items = items
        };
    }
}
