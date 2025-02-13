﻿using AutoMapper;
using MediatR;
using WebAPI.Core.DTOs;
using WebAPI.Core.Queries.ProductQueries;
using WebAPI.Core.Repository;

namespace WebAPI.Core.Handlers.Products
{
    internal class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
    {
        private readonly IProductRepository _repository;
        private IMapper _mapper;

        public GetAllProductsHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllProductsAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }
    }
}
