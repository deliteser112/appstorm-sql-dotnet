using appstorm_sql_dotnet_test.Core.Entities;
using appstorm_sql_dotnet_test.Core.Interfaces;
using appstorm_sql_dotnet_test.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using appstorm_sql_dotnet_test.Presentation.DTOs;

namespace appstorm_sql_dotnet_test.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        // GET: /products
        // Retrieves all products.
        [HttpGet(Name = "GetProducts")]
        public async Task<IEnumerable<ProductDto>> Get()
        {
            _logger.LogInformation("Retrieving all products");
            var products = await _productService.GetProductsAsync();

            // Maps the list of Product entities to ProductDto objects before returning.
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            });
        }

        // GET: /products/{id}
        // Retrieves a product by its ID.
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Retrieving product with ID: {id}");
            var product = await _productService.GetProductByIdAsync(id);

            // If product is not found, returns a 404 NotFound response.
            if (product == null)
            {
                _logger.LogWarning($"Product with ID: {id} not found");
                return NotFound();
            }

            // Converts the Product entity to a ProductDto before returning.
            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };

            return Ok(productDto);
        }

        // POST: /products
        // Creates a new product based on the ProductDto provided.
        [HttpPost(Name = "CreateProduct")]
        public async Task<IActionResult> Post([FromBody] ProductDto productDto)
        {
            // Validates the incoming request.
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid product data");
                return BadRequest(ModelState);
            }

            // Maps ProductDto to the Product entity and passes it to the service layer.
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock
            };

            await _productService.AddProductAsync(product);

            _logger.LogInformation($"Product {product.Name} created successfully");

            // Returns a 201 Created status with the route to get the newly created product.
            return CreatedAtRoute("GetProductById", new { id = product.Id }, productDto);
        }

        // PUT: /products/{id}
        // Updates an existing product. Returns NoContent if successful, BadRequest if the IDs don't match.
        [HttpPut("{id}", Name = "UpdateProduct")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductDto productDto)
        {
            // Checks if the product ID matches and if the model is valid.
            if (id != productDto.Id || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid product update request");
                return BadRequest();
            }

            var product = new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock
            };

            await _productService.UpdateProductAsync(product);
            _logger.LogInformation($"Product with ID: {id} updated successfully");

            // Returns a 204 NoContent status as the update was successful.
            return NoContent();
        }

        // DELETE: /products/{id}
        // Deletes a product by its ID. Returns NoContent if successful, or NotFound if the product does not exist.
        [HttpDelete("{id}", Name = "DeleteProduct")]
        public async Task<IActionResult> Delete(int id)
        {
            // Checks if the product exists before attempting deletion.
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID: {id} not found for deletion");
                return NotFound();
            }

            // Deletes the product and logs the action.
            await _productService.DeleteProductAsync(id);
            _logger.LogInformation($"Product with ID: {id} deleted successfully");

            // Returns a 204 NoContent status to indicate successful deletion.
            return NoContent();
        }
    }
}
