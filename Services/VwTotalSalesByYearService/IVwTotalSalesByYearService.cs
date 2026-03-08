using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwTotalSalesByYearService;

public interface IVwTotalSalesByYearService
{
    Task<PagedResult<VwTotalSalesByYear>> GetAllAsync(int top = 50, string order = "desc");
}
