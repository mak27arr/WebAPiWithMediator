using Grpc.Core;
using MediatR;
using ProductService.Grpc;
using Products.Core.Queries.ProductQueries;
using Serilog.Context;

namespace Products.ProductAPI.Services
{
    public class ProductGrpcService : ProductService.Grpc.ProductService.ProductServiceBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductGrpcService> _logger;

        public ProductGrpcService(IMediator mediator, ILogger<ProductGrpcService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public override async Task<ProductResponse> GetProductById(ProductRequest request, ServerCallContext context)
        {
            var sessionId = GetSessionId(context);
            _logger.LogInformation("Received gRPC request for ProductId {ProductId} with SessionId: {SessionId}", request.ProductId, sessionId);

            using (LogContext.PushProperty("SessionId", sessionId))
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

        private static string GetSessionId(ServerCallContext context)
        {
            return context.RequestHeaders.GetValue("X-Session-Id") ?? "UnknownSessionId";
        }
    }
}
