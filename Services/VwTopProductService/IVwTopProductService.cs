using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwTopProductService;

public interface IVwTopProductService
{
    Task<PagedResult<VwTopProduct>> GetAllAsync(int top = 50, string order = "desc", string? category = null);
}
