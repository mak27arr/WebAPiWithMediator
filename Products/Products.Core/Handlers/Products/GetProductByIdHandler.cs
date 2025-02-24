using AutoMapper;
using MediatR;
using Products.Core.DTOs;
using Products.Core.Queries.ProductQueries;
using Products.Infrastructure.Interfaces.Repository;

namespace Products.Core.Handlers.Products
{
    internal class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDTO>
    {
        private readonly IProductRepository _repository;
        private IMapper _mapper;

        public GetProductByIdHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductByIdAsync(request.Id);
            return _mapper.Map<ProductDTO>(product);
        }
    }
}
