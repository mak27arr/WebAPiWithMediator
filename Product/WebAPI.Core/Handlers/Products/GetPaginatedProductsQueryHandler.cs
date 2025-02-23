﻿using AutoMapper;
using MediatR;
using Products.Common.Type.Page;
using WebAPI.Core.DTOs;
using WebAPI.Core.Queries.ProductQueries;
using WebAPI.Core.Repository;

namespace WebAPI.Core.Handlers.Products
{
    internal class GetPaginatedProductsQueryHandler : IRequestHandler<GetPaginatedProductsQuery, PagedResult<ProductDTO>>
    {
        private readonly IProductRepository _repository;
        private IMapper _mapper;

        public GetPaginatedProductsQueryHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductDTO>> Handle(GetPaginatedProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetPaginatedProductsAsync(request.PageIndex, request.PageSize);
            var pagesCount = await _repository.GetPageCountAsync(request.PageSize);
            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return new PagedResult<ProductDTO>()
            {
                Items = productsDto,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalPages = pagesCount
            };
        }
    }
}
