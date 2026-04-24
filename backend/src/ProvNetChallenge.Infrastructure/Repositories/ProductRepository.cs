using Microsoft.EntityFrameworkCore;
using ProvNetChallenge.Application.Interfaces.Repositories;
using ProvNetChallenge.Domain.Entities;
using ProvNetChallenge.Infrastructure.Data;

namespace ProvNetChallenge.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // Solo traemos los activos (borrado lógico)
            return await _dbContext.PRODUCT
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _dbContext.PRODUCT
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<Product?> GetBySkuAsync(string sku)
        {
            // Esto nos va a servir en el Service para que no tire error SQL por el índice único
            return await _dbContext.PRODUCT
                .FirstOrDefaultAsync(p => p.SKU == sku);
        }

        public async Task AddAsync(Product product)
        {
            await _dbContext.PRODUCT.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _dbContext.PRODUCT.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}