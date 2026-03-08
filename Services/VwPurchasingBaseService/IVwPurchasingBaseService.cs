using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwPurchasingBaseService;

public interface IVwPurchasingBaseService
{
    Task<PagedResult<VwPurchasingBase>> GetAllAsync(int top = 50, string order = "desc", string? category = null);
}
