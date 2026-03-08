using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwPurchasingByVendorService;

public interface IVwPurchasingByVendorService
{
    Task<PagedResult<VwPurchasingByVendor>> GetAllAsync(int top = 50, string order = "desc", string? category = null);
}
