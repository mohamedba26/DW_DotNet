using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.FactPurchasingService
{
    public class FactPurchasingService : IFactPurchasingService
    {
        private readonly DwSalesPurchasingContext _context;

        public FactPurchasingService(DwSalesPurchasingContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<FactPurchasingDto>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            var baseQuery = _context.FactPurchasings
                .AsNoTracking()
                .Include(f => f.ProductKeyNavigation)
                .Include(f => f.VendorKeyNavigation);

            var total = await baseQuery.LongCountAsync();

            var entities = await baseQuery
                .OrderBy(f => f.PurchasingKey)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(FactPurchasingDto.FromFactPurchasing).ToList();

            return new PagedResult<FactPurchasingDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            };
        }
    }
}
