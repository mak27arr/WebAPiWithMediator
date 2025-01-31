using MediatR;
using WebAPi.Core.Commands;
using WebAPI.Core.Models;
using WebAPi.Core.Repository;

namespace WebAPi.Core.Handlers
{
    internal class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IProductRepository _repository;

        public UpdateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = request.Id,
                Name = request.Name
            };

            return await _repository.UpdateProductAsync(product);
        }
    }
}
