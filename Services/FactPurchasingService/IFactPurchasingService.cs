using SalesDW.API.Models;
using SalesDW.API.Models.DW.Tables;
using System.Threading.Tasks;

namespace SalesDW.API.Services.FactPurchasingService
{
    public interface IFactPurchasingService
    {
        Task<PagedResult<FactPurchasingDto>> GetAllAsync(int page = 1, int pageSize = 50);
    }
}
