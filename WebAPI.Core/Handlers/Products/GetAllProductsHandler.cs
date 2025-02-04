using MediatR;
using WebAPI.Core.Models.Products;
using WebAPI.Core.Queries.ProductQueries;
using WebAPI.Core.Repository;

namespace WebAPI.Core.Handlers.Products
{
    internal class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllProductsAsync();
        }
    }
}
