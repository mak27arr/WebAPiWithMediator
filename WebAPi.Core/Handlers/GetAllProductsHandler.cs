using MediatR;
using WebAPI.Core.Models;
using WebAPi.Core.Queries;
using WebAPi.Core.Repository;

namespace WebAPi.Core.Handlers
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
