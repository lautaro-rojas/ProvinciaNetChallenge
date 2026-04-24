using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProvNetChallenge.Application.DTOs;
using ProvNetChallenge.Application.Interfaces;

namespace ProvNetChallenge.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ProductGetAll()
        {
            var products = await _productService.GetAllAsync();
            
            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "No products found." });
            }
            
            return Ok(products); // Devuelve 200 OK con la lista
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ProductGetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            
            if (product == null)
                return NotFound(new { message = $"Product with ID {id} not found or is inactive." }); // Devuelve 404

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ProductCreate([FromBody] ProductCreationDto dto)
        {
            // Si el DTO no cumple los [Required], .NET devuelve 400 automáticamente acá.
            // Si el SKU está repetido, nuestro Service lanza BadRequestException y lo ataja el Middleware.
            
            var newProductId = await _productService.AddAsync(dto);

            return CreatedAtAction(nameof(ProductGetById), new { id = newProductId }, new { id = newProductId, message = "Product created successfully." });
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ProductUpdate([FromRoute] int id, [FromBody] ProductUpdateDto dto)
        {
            var success = await _productService.UpdateAsync(id, dto);
            
            if (!success)
                return NotFound(new { message = $"Product with ID {id} not found." });

            return NoContent(); // Devuelve 204 No Content (Estándar REST para un update exitoso)
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var deleted = await _productService.DeleteLogicAsync(id);
            
            if (!deleted)
                return NotFound(new { message = $"Product with ID {id} not found." });

            return NoContent(); // Devuelve 204 No Content
        }
    }
}