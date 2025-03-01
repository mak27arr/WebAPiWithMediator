using AutoMapper;
using MediatR;
using OneOf;
using OneOf.Types;
using Products.Common.Type.Page;
using Products.Core.Commands.Products;
using Products.Core.DTOs;
using Products.Infrastructure.Interfaces.Caching;
using Products.Infrastructure.Interfaces.Repository;

namespace Products.Core.Handlers.Products.Commands
{
    internal class UpdateProductHandler : IRequestHandler<UpdateProductCommand, OneOf<Success, NotFound>>
    {
        private readonly IMapper _mapper;
        private readonly ICacheService<PagedResult<ProductDTO>> _cahce;
        private readonly IProductRepository _repository;

        public UpdateProductHandler(IMapper mapper, ICacheService<PagedResult<ProductDTO>> cahce, IProductRepository repository)
        {
            _mapper = mapper;
            _cahce = cahce;
            _repository = repository;
        }

        public async Task<OneOf<Success, NotFound>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductByIdAsync(request.Id);

            if (product == null)
                return new NotFound();

            _mapper.Map(request, product);
            await _repository.UpdateProductAsync(product);
            await _cahce.InvalidateCacheAsync();

            return new Success();
        }
    }
}
