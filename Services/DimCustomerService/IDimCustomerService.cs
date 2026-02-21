using SalesDW.API.Models;
using System.Threading.Tasks;

namespace SalesDW.API.Services.DimCustomerService
{
    public interface IDimCustomerService
    {
        Task<PagedResult<DimCustomerDto>> GetAllAsync(int page = 1, int pageSize = 50);
        Task<DimCustomerDto?> GetByIdAsync(int id);
    }
}
