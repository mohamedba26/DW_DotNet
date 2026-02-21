using SalesDW.API.Models;
using System.Threading.Tasks;

namespace SalesDW.API.Services.DimDateService
{
    public interface IDimDateService
    {
        Task<PagedResult<DimDate>> GetAllAsync(int page = 1, int pageSize = 50);
        Task<DimDate?> GetByIdAsync(int id);
    }
}
