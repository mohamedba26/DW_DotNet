using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwProductProfitService;

public interface IVwProductProfitService
{
    Task<PagedResult<VwProductProfit>> GetAllAsync(int top = 50, string order = "desc", string? category = null);
    Task<VwProductProfit?> GetByProductKeyAsync(int productKey);
}
