using SalesDW.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesDW.API.Services.AuthProductService
{
    public interface IAuthProductService
    {
        Task<IEnumerable<AuthProduct>> GetAllAsync();
        Task<AuthProduct?> GetByIdAsync(int id);
        Task<AuthProduct> CreateAsync(AuthProduct p);
        Task<AuthProduct?> UpdateAsync(int id, AuthProduct p);
        Task<bool> DeleteAsync(int id);
    }
}
