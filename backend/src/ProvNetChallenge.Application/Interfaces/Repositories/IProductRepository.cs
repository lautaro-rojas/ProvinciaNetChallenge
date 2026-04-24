using ProvNetChallenge.Domain.Entities;

namespace ProvNetChallenge.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);
        
        Task<Product?> GetBySkuAsync(string sku); 
        
        Task AddAsync(Product product);
        
        Task UpdateAsync(Product product);
    }
}