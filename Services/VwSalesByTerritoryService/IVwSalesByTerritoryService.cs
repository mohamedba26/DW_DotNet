using SalesDW.API.Models;
using SalesDW.API.Models.DW.Views;
using System.Threading.Tasks;

namespace SalesDW.API.Services.VwSalesByTerritoryService;

public interface IVwSalesByTerritoryService
{
    Task<PagedResult<VwSalesByTerritory>> GetAllAsync(int top = 50, string order = "desc", string? territory = null);
}
