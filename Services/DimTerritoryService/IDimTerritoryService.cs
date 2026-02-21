using SalesDW.API.Models;
using System.Threading.Tasks;

namespace SalesDW.API.Services.DimTerritoryService
{
    public interface IDimTerritoryService
    {
        Task<PagedResult<DimTerritory>> GetAllAsync(int page = 1, int pageSize = 50);
        Task<DimTerritory?> GetByIdAsync(int id);
    }
}
