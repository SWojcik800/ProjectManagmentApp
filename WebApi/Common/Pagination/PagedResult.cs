namespace DapperSamples.Common.Pagination
{
    public class PagedResult<T> where T : class
    {
        public PagedResult(int? totalCount, IEnumerable<T> items)
        {
            TotalCount = totalCount is not null ? totalCount.Value : items.Count();
            Items = items;
        }

        public int TotalCount { get; init; }
        public IEnumerable<T> Items { get; init; }

    }
}
