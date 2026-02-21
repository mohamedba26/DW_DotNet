using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.DimVendorService
{
    public class DimVendorService : IDimVendorService
    {
        private readonly DwSalesPurchasingContext _context;

        public DimVendorService(DwSalesPurchasingContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<DimVendor>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            var query = _context.DimVendors.AsNoTracking();
            var total = await query.LongCountAsync();

            var items = await query
                .OrderBy(v => v.VendorKey)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<DimVendor>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            };
        }

        public async Task<DimVendor?> GetByIdAsync(int id)
        {
            return await _context.DimVendors.AsNoTracking().FirstOrDefaultAsync(v => v.VendorKey == id);
        }
    }
}
