using SalesDW.API.Models;
using SalesDW.API.Models.DW.Tables;
using System.Threading.Tasks;

namespace SalesDW.API.Services.DimProductService
{
    public interface IDimProductService
    {
        Task<PagedResult<DimProduct>> GetAllAsync(int page = 1, int pageSize = 50);
        Task<DimProduct?> GetByIdAsync(int id);
    }
}
