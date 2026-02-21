using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.DimCustomerService
{
    public class DimCustomerService : IDimCustomerService
    {
        private readonly DwSalesPurchasingContext _context;

        public DimCustomerService(DwSalesPurchasingContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<DimCustomerDto>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            var query = _context.DimCustomers.AsNoTracking();

            var total = await query.LongCountAsync();

            var items = await query
                .OrderBy(c => c.CustomerKey)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new DimCustomerDto
                {
                    CustomerKey = c.CustomerKey,
                    FullName = c.FullName,
                    EmailAddress = c.EmailAddress,
                    CustomerType = c.CustomerType
                })
                .ToListAsync();

            return new PagedResult<DimCustomerDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            };
        }

        public async Task<DimCustomerDto?> GetByIdAsync(int id)
        {
            var c = await _context.DimCustomers
                .AsNoTracking()
                .Where(x => x.CustomerKey == id)
                .Select(x => new DimCustomerDto
                {
                    CustomerKey = x.CustomerKey,
                    FullName = x.FullName,
                    EmailAddress = x.EmailAddress,
                    CustomerType = x.CustomerType
                })
                .FirstOrDefaultAsync();

            return c;
        }
    }
}
