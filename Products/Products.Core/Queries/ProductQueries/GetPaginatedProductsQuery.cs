using MediatR;
using Products.Common.Type.Page;
using Products.Core.DTOs;

namespace Products.Core.Queries.ProductQueries
{
    public class GetPaginatedProductsQuery : IRequest<PagedResult<ProductDTO>>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
