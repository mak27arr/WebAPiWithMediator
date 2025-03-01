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
    internal class DeleteProductHandler : IRequestHandler<DeleteProductCommand, OneOf<Success, NotFound>>
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService<PagedResult<ProductDTO>> _cahce;

        public DeleteProductHandler(IProductRepository repository, ICacheService<PagedResult<ProductDTO>> cahce) 
        {
            _repository = repository;
            _cahce = cahce;
        }

        public async Task<OneOf<Success, NotFound>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _repository.DeleteProductAsync(request.Id);

            if (deleted)
            {
                await _cahce.InvalidateCacheAsync();
                return new Success();
            }
            else
            {
                return new NotFound();
            }
        }
    }
}
