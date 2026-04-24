using ProvNetChallenge.Domain.Entities;
using ProvNetChallenge.Application.Interfaces;
using ProvNetChallenge.Application.DTOs;
using ProvNetChallenge.Application.Interfaces.Repositories;
using ProvNetChallenge.Application.Exceptions;

namespace ProvNetChallenge.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> AddAsync(ProductCreationDto dto)
        {
            var existingProduct = await _productRepository.GetBySkuAsync(dto.SKU);
            if (existingProduct != null)
            {
                // El SKU debe ser único, si ya existe lanzamos una Custom Exception (BadRequestException).
                throw new BadRequestException("The SKU already exists, must be unique.");
            }

            var newProduct = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                SKU = dto.SKU,
                Price = dto.Price,
                Stock = dto.Stock,
                IsActive = true
            };

            await _productRepository.AddAsync(newProduct);
            return newProduct.Id;
        }

        public async Task<bool> DeleteLogicAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            product.IsActive = false;
            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                SKU = p.SKU,
                Price = p.Price,
                Stock = p.Stock,
                IsActive = p.IsActive,
            }).ToList();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Price = product.Price,
                Stock = product.Stock,
                IsActive = product.IsActive
            };
        }

        public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;

            await _productRepository.UpdateAsync(product);
            return true;
        }
    }
}