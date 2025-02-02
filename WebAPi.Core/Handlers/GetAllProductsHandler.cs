using MediatR;
using WebAPI.Core.Models;
using WebAPI.Core.Queries;
using WebAPI.Core.Repository;

namespace WebAPI.Core.Handlers
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
