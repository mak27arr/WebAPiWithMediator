using Grpc.Core;
using MediatR;
using ProductService.Grpc;
using Products.Core.Queries.ProductQueries;

namespace Products.ProductAPI.Services
{
    public class ProductGrpcService : ProductService.Grpc.ProductService.ProductServiceBase
    {
        private readonly IMediator _mediator;

        public ProductGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<ProductResponse> GetProductById(ProductRequest request, ServerCallContext context)
        {
            var product = await _mediator.Send(new GetProductByIdQuery() { Id = request.ProductId });

            if (product == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));

            return new ProductResponse() 
            { 
                ProductId = product.Id,
                ProductName = product.Name
            };
        }
    }
}
