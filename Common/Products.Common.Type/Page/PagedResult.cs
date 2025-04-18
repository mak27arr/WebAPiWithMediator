namespace Products.Common.Type.Page
{
    public class PagedResult<T>
    {
        public required IEnumerable<T> Items { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}
