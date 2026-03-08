using SalesDW.API.Models;
using SalesDW.API.Models.DW.Tables;
using System.Threading.Tasks;

namespace SalesDW.API.Services.FactSaleService
{
    public interface IFactSaleService
    {
        Task<PagedResult<FactSaleDto>> GetAllAsync(int page = 1, int pageSize = 50);
    }
}
