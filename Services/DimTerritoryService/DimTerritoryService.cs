using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDW.API.Services.DimTerritoryService
{
    public class DimTerritoryService : IDimTerritoryService
    {
        private readonly DwSalesPurchasingContext _context;

        public DimTerritoryService(DwSalesPurchasingContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<DimTerritory>> GetAllAsync(int page = 1, int pageSize = 50)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            var query = _context.DimTerritories.AsNoTracking();
            var total = await query.LongCountAsync();

            var items = await query
                .OrderBy(t => t.TerritoryKey)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<DimTerritory>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            };
        }

        public async Task<DimTerritory?> GetByIdAsync(int id)
        {
            return await _context.DimTerritories.AsNoTracking().FirstOrDefaultAsync(t => t.TerritoryKey == id);
        }
    }
}
