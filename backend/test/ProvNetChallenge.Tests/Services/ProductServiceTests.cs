using Moq;
using ProvNetChallenge.Application.DTOs;
using ProvNetChallenge.Application.Exceptions;
using ProvNetChallenge.Application.Interfaces.Repositories;
using ProvNetChallenge.Application.Services;
using ProvNetChallenge.Domain.Entities;

namespace ProvNetChallenge.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly ProductService _productService;

        // El constructor en xUnit actúa como el "Setup". Se ejecuta antes de cada test.
        public ProductServiceTests()
        {
            // Creamos un repositorio "mentiroso" que no se conecta a SQL Server
            _mockRepository = new Mock<IProductRepository>();
            
            // Le inyectamos la mentira a nuestro servicio real
            _productService = new ProductService(_mockRepository.Object);
        }

        [Fact] 
        public async Task AddAsync_WithExistingSku_ShouldThrowBadRequestException()
        {
            // 1. ARRANGE (Preparar)
            var dto = new ProductCreationDto { SKU = "SKU-REPETIDO", Name = "Mouse" };
            
            // Le enseñamos al mock a mentir: "Si te piden este SKU, devolvé un producto (simulando que ya existe)"
            _mockRepository.Setup(repo => repo.GetBySkuAsync(dto.SKU))
                           .ReturnsAsync(new Product { SKU = "SKU-REPETIDO" });

            // 2 & 3. ACT & ASSERT (Actuar y Afirmar)
            // Verificamos que al llamar al método, el servicio lance la excepción correcta
            await Assert.ThrowsAsync<BadRequestException>(() => _productService.AddAsync(dto));
            
            // Verificamos que JAMÁS se haya llamado al método de guardado en la BD
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_WithNewSku_ShouldReturnNewProductId()
        {
            // 1. ARRANGE
            var dto = new ProductCreationDto { SKU = "SKU-NUEVO", Name = "Teclado", Price = 100, Stock = 10 };
            
            // Le decimos al mock: "Si te piden este SKU, devolvé NULL (simulando que no existe en la BD)"
            _mockRepository.Setup(repo => repo.GetBySkuAsync(dto.SKU))
                           .ReturnsAsync((Product?)null);

            // Simulamos el AddAsync para que cuando el servicio intente guardarlo, le asigne el ID 1
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                           .Callback<Product>(p => p.Id = 1)
                           .Returns(Task.CompletedTask);

            // 2. ACT
            var resultId = await _productService.AddAsync(dto);

            // 3. ASSERT
            Assert.Equal(1, resultId); // El servicio nos debería devolver el ID 1
            
            // Verificamos que el repositorio haya sido llamado exactamente UNA vez para guardar
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}