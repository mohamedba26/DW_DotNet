using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.FactSaleService
{
    public class FactSaleService : IFactSaleService
    {
        private readonly DwSalesPurchasingContext _context;

        public FactSaleService(DwSalesPurchasingContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<FactSaleDto>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            var baseQuery = _context.FactSales
                .AsNoTracking()
                .Include(s => s.ProductKeyNavigation)
                .Include(s => s.CustomerKeyNavigation)
                .Include(s => s.TerritoryKeyNavigation)
                .Include(s => s.OrderDateKeyNavigation);

            var total = await baseQuery.LongCountAsync();

            var entities = await baseQuery
                .OrderBy(s => s.SalesKey)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(FactSaleDto.FromFactSale).ToList();

            return new PagedResult<FactSaleDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            };
        }
    }
}
