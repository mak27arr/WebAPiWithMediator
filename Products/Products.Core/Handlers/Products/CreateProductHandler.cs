using MediatR;
using Products.Core.Commands.Products;
using Products.Core.DTOs;
using Products.Infrastructure.Models;
using AutoMapper;
using Products.Infrastructure.Interfaces.Repository;

namespace Products.Core.Handlers.Products
{
    internal class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDTO>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public CreateProductHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name
            };

            var createdProduct = await _repository.AddProductAsync(product);
            return _mapper.Map<ProductDTO>(createdProduct);
        }
    }
}
