using Microsoft.EntityFrameworkCore;
using SalesDW.API.Data;
using SalesDW.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesDW.API.Services.AuthProductService
{
    public class AuthProductService : IAuthProductService
    {
        private readonly AuthDbContext _context;

        public AuthProductService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuthProduct>> GetAllAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<AuthProduct?> GetByIdAsync(int id)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<AuthProduct> CreateAsync(AuthProduct p)
        {
            _context.Products.Add(p);
            await _context.SaveChangesAsync();
            return p;
        }

        public async Task<AuthProduct?> UpdateAsync(int id, AuthProduct p)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null) return null;

            existing.ProductName = p.ProductName;
            existing.StandardCost = p.StandardCost;
            existing.ListPrice = p.ListPrice;
            existing.Subcategory = p.Subcategory;
            existing.Category = p.Category;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null) return false;

            _context.Products.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
