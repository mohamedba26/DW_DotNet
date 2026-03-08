using SalesDW.API.Models;
using System.Threading.Tasks;

namespace SalesDW.API.Services.KpiService;

public interface IKpiService
{
    Task<KpiResult> GetKpisAsync();
}
