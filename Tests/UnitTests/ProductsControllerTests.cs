using Xunit;
using Moq;
using appstorm_sql_dotnet_test.Core.Interfaces;
using appstorm_sql_dotnet_test.Application.Services;
using appstorm_sql_dotnet_test.Core.Controllers;
using appstorm_sql_dotnet_test.Core.Entities;
using appstorm_sql_dotnet_test.Presentation.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace appstorm_sql_dotnet_test.Tests.UnitTests
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly Mock<IProductService> _mockService;
        private readonly Mock<ILogger<ProductsController>> _mockLogger;

        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100, Stock = 10 },
                new Product { Id = 2, Name = "Product 2", Price = 200, Stock = 20 }
            };

            _mockService.Setup(service => service.GetProductsAsync())
                        .ReturnsAsync(products);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 100, Stock = 10 };

            _mockService.Setup(service => service.GetProductByIdAsync(1))
                        .ReturnsAsync(product);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetProductByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsCreatedResult_WithProduct()
        {
            // Arrange
            var productDto = new ProductDto { Name = "Product 1", Price = 100, Stock = 10 };
            var product = new Product { Id = 1, Name = "Product 1", Price = 100, Stock = 10 };

            _mockService.Setup(service => service.AddProductAsync(It.IsAny<Product>()))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(productDto);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<ProductDto>(createdAtRouteResult.Value);
            Assert.Equal("Product 1", returnValue.Name);
        }

        [Fact]
        public async Task Put_ReturnsNoContentResult_WhenProductIsUpdated()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Updated Product", Price = 150, Stock = 20 };
            var product = new Product { Id = 1, Name = "Updated Product", Price = 150, Stock = 20 };

            _mockService.Setup(service => service.UpdateProductAsync(It.IsAny<Product>()))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(1, productDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenProductIdsDoNotMatch()
        {
            // Arrange
            var productDto = new ProductDto { Id = 2, Name = "Updated Product", Price = 150, Stock = 20 };

            // Act
            var result = await _controller.Put(1, productDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult_WhenProductIsDeleted()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 100, Stock = 10 };

            _mockService.Setup(service => service.GetProductByIdAsync(1))
                        .ReturnsAsync(product);

            _mockService.Setup(service => service.DeleteProductAsync(1))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetProductByIdAsync(1))
                        .ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
