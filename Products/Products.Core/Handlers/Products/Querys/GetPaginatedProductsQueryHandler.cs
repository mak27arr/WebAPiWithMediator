using AutoMapper;
using MediatR;
using Products.Common.Type.Page;
using Products.Core.DTOs;
using Products.Core.Queries.ProductQueries;
using Products.Infrastructure.Interfaces.Caching;
using Products.Infrastructure.Interfaces.Repository;

namespace Products.Core.Handlers.Products.Query
{
    internal class GetPaginatedProductsQueryHandler : IRequestHandler<GetPaginatedProductsQuery, PagedResult<ProductDTO>>
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService<PagedResult<ProductDTO>> _productPageCache;
        private IMapper _mapper;

        public GetPaginatedProductsQueryHandler(IProductRepository repository, ICacheService<PagedResult<ProductDTO>> productPageCache, IMapper mapper)
        {
            _repository = repository;
            _productPageCache = productPageCache;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductDTO>> Handle(GetPaginatedProductsQuery request, CancellationToken cancellationToken)
        {
            var key = GetQueryKey(request);
            var productsPage = await _productPageCache.GetCacheAsync(key);

            if (productsPage is not null)
                return productsPage;

            var products = await _repository.GetPaginatedProductsAsync(request.PageIndex, request.PageSize);
            var pagesCount = await _repository.GetPageCountAsync(request.PageSize);
            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            productsPage = new PagedResult<ProductDTO>()
            {
                Items = productsDto,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalPages = pagesCount
            };

            await _productPageCache.SetCacheAsync(key, productsPage, TimeSpan.FromHours(1));

            return productsPage;
        }

        private string GetQueryKey(GetPaginatedProductsQuery request)
        {
            return $"{request.PageSize}_{request.PageIndex}";
        }
    }
}
