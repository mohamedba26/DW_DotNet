using SalesDW.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesDW.API.Services.StatisticsService
{
    public interface IStatisticsService
    {
        Task<IEnumerable<PurchasingByVendorDto>> GetPurchasingByVendorAsync(string sort = "desc", string? category = null);
        Task<IEnumerable<TopProductDto>> GetTopProductsAsync(int top = 10, string sort = "desc");
        Task<IEnumerable<SalesByTerritoryDto>> GetSalesByTerritoryAsync(string sort = "desc", string? category = null);
        Task<IEnumerable<SalesByYearDto>> GetSalesByYearAsync(string sort = "desc", string? category = null);
        Task<IEnumerable<SalesByVendorDto>> GetSalesByVendorAsync(string sort = "desc", int top = 0, string? category = null);

        Task<IEnumerable<ProductProfitDto>> GetProductsByProfitAsync(int top = 10, string? category = null, string sort = "desc");

        Task<IEnumerable<TimeSeriesPointDto>> GetTimeSeriesAsync(string metric = "sales", string period = "month", int months = 12, string? category = null);
    }
}
