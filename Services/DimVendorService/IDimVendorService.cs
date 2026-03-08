using SalesDW.API.Models;
using SalesDW.API.Models.DW.Tables;
using System.Threading.Tasks;

namespace SalesDW.API.Services.DimVendorService
{
    public interface IDimVendorService
    {
        Task<PagedResult<DimVendor>> GetAllAsync(int page = 1, int pageSize = 50);
        Task<DimVendor?> GetByIdAsync(int id);
    }
}
