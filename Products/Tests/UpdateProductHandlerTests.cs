using Moq;
using Xunit;
using OneOf.Types;
using AutoMapper;
using Products.Core.Commands.Products;
using Products.Infrastructure.Models;
using Products.Infrastructure.Interfaces.Repository;
using Products.Core.Handlers.Products.Commands;
using Products.Core.DTOs;
using Products.Common.Type.Page;
using Products.Infrastructure.Interfaces.Caching;

namespace Products.Tests
{
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            var mockCache = new Mock<ICacheService<PagedResult<ProductDTO>>>();

            var dummyData = new PagedResult<ProductDTO>
            {
                Items = new List<ProductDTO>
                {
                    new ProductDTO { Name = "TestProduct1"},
                    new ProductDTO { Name = "TestProduct2"}
                },
                TotalPages = 2
            };

            mockCache.Setup(c => c.GetCacheAsync(It.IsAny<string>()))
                     .ReturnsAsync(dummyData);

            mockCache.Setup(c => c.SetCacheAsync(It.IsAny<string>(), It.IsAny<PagedResult<ProductDTO>>(), It.IsAny<TimeSpan?>()))
                     .Returns(Task.CompletedTask);

            mockCache.Setup(c => c.InvalidateCacheAsync())
                     .Returns(Task.CompletedTask);

            mockCache.Setup(c => c.InvalidateCacheForTypeAsync(It.IsAny<Type>()))
                     .Returns(Task.CompletedTask);

            _handler = new UpdateProductHandler(_mockMapper.Object, mockCache.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ReturnsNotFound_Test()
        {
            var command = new UpdateProductCommand() { Id = 1, Name = "New Product" };
            _mockRepository.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(null as Product);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<NotFound>(result.Value);
        }

        [Fact]
        public async Task Handle_ProductFound_UpdatesProductAndReturnsSuccess_Test()
        {
            var command = new UpdateProductCommand() { Id = 1, Name = "Updated Product" };
            var existingProduct = new Product { Id = 1, Name = "Old Product" };
            _mockRepository.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(existingProduct);
            _mockMapper.Setup(m => m.Map(It.IsAny<UpdateProductCommand>(), It.IsAny<Product>()))
                .Callback<UpdateProductCommand, Product>((cmd, product) =>
                {
                    product.Name = cmd.Name;
                });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<Success>(result.Value);
            _mockRepository.Verify(repo => repo.UpdateProductAsync(existingProduct), Times.Once);
            Assert.Equal("Updated Product", existingProduct.Name);
        }

        [Fact]
        public async Task Handle_UpdateNotExistedProduct_Test()
        {
            var command = new UpdateProductCommand() { Id = 10, Name = "Update Product" };
            var existingProduct = new Product { Id = 1, Name = "Exist Product" };
            _mockRepository.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(existingProduct);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<NotFound>(result.Value);
            _mockRepository.Verify(repo => repo.GetProductByIdAsync(command.Id), Times.Once);
            _mockRepository.Verify(repo => repo.UpdateProductAsync(It.IsAny<Product>()), Times.Never);
        }
    }

}