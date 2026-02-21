using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.DimProductService
{
    public class DimProductService : IDimProductService
    {
        private readonly DwSalesPurchasingContext _context;

        public DimProductService(DwSalesPurchasingContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<DimProduct>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            var query = _context.DimProducts.AsNoTracking();
            var total = await query.LongCountAsync();

            var items = await query
                .OrderBy(p => p.ProductKey)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<DimProduct>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            };
        }

        public async Task<DimProduct?> GetByIdAsync(int id)
        {
            return await _context.DimProducts.AsNoTracking().FirstOrDefaultAsync(p => p.ProductKey == id);
        }
    }
}
