using MediatR;
using WebAPI.Core.Models.Products;
using WebAPI.Core.Queries.ProductQueries;
using WebAPI.Core.Repository;

namespace WebAPI.Core.Handlers.Products
{
    internal class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetProductByIdAsync(request.Id);
        }
    }
}
