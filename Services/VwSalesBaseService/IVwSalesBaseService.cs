using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwSalesBaseService;

public interface IVwSalesBaseService
{
    Task<PagedResult<VwSalesBase>> GetAllAsync(int top = 50, string order = "desc", string? category = null);
}
