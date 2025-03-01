using MediatR;
using Products.Core.Commands.Products;
using Products.Core.DTOs;
using Products.Infrastructure.Models;
using AutoMapper;
using Products.Infrastructure.Interfaces.Repository;
using Products.Infrastructure.Interfaces.Caching;
using Products.Common.Type.Page;

namespace Products.Core.Handlers.Products.Commands
{
    internal class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDTO>
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService<PagedResult<ProductDTO>> _cahce;
        private readonly IMapper _mapper;

        public CreateProductHandler(IProductRepository repository, ICacheService<PagedResult<ProductDTO>> cahce, IMapper mapper) 
        {
            _repository = repository;
            _cahce = cahce;
            _mapper = mapper;
        }

        public async Task<ProductDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name
            };

            var createdProduct = await _repository.AddProductAsync(product);
            await _cahce.InvalidateCacheAsync();

            return _mapper.Map<ProductDTO>(createdProduct);
        }
    }
}
