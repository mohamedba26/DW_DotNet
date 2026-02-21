using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.DimDateService
{
    public class DimDateService : IDimDateService
    {
        private readonly DwSalesPurchasingContext _context;

        public DimDateService(DwSalesPurchasingContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<DimDate>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            var query = _context.DimDates.AsNoTracking();
            var total = await query.LongCountAsync();

            var items = await query
                .OrderBy(d => d.DateKey)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<DimDate>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            };
        }

        public async Task<DimDate?> GetByIdAsync(int id)
        {
            return await _context.DimDates.AsNoTracking().FirstOrDefaultAsync(d => d.DateKey == id);
        }
    }
}
