using Grpc.Core;
using System.Diagnostics;

namespace Products.Common.Protos.Product
{
    public class ProductGrpcService : IProductGrpcService
    {
        private readonly ProductService.Grpc.ProductService.ProductServiceClient _client;

        public ProductGrpcService(ProductService.Grpc.ProductService.ProductServiceClient client)
        {
            _client = client;
        }

        public async Task<bool> ProductExistsAsync(int productId)
        {
            var request = new ProductService.Grpc.ProductRequest { ProductId = productId };
            var metadata = GetMetadata();
            var response = await _client.GetProductByIdAsync(request, metadata);

            return response?.ProductId != null;
        }

        private static Metadata GetMetadata()
        {
            var sessionId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();
            var metadata = new Metadata { { "X-Session-Id", sessionId } };
            return metadata;
        }
    }
}
