using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using System.Threading.Tasks;

namespace SalesDW.API.Services.KpiService;

public class KpiService : IKpiService
{
    private readonly DwSalesPurchasingContext _context;
    private readonly AuthDbContext _authContext;

    public KpiService(DwSalesPurchasingContext context, AuthDbContext authContext)
    {
        _context = context;
        _authContext = authContext;
    }

    public async Task<KpiResult> GetKpisAsync()
    {
        var totalSales = await _context.FactSales.AsNoTracking().SumAsync(s => (decimal?)s.LineTotal) ?? 0m;
        var totalCustomers = await _context.DimCustomers.AsNoTracking().LongCountAsync();
        var totalProducts = await _context.DimProducts.AsNoTracking().LongCountAsync();

        return new KpiResult
        {
            TotalSales = totalSales,
            TotalCustomers = totalCustomers,
            TotalProducts = totalProducts
        };
    }
}
