using MediatR;
using Products.Common.Type.Page;
using WebAPI.Core.DTOs;

namespace WebAPI.Core.Queries.ProductQueries
{
    public class GetPaginatedProductsQuery : IRequest<PagedResult<ProductDTO>>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
