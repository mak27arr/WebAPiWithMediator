﻿using Grpc.Core;
using MediatR;
using ProductService.Grpc;
using WebAPI.Core.Queries.ProductQueries;

namespace WebAPI.ProductAPI.Services
{
    public class ProductGrpcService : ProductService.Grpc.ProductService.ProductServiceBase
    {
        private readonly IMediator _mediator;

        public ProductGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<ProductResponse> CheckProductExists(ProductRequest request, ServerCallContext context)
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
