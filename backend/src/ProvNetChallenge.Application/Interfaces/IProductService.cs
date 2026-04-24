using ProvNetChallenge.Application.DTOs;

namespace ProvNetChallenge.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<int> AddAsync(ProductCreationDto dto);
        Task<bool> UpdateAsync(int id, ProductUpdateDto dto);
        Task<bool> DeleteLogicAsync(int id);
    }
}