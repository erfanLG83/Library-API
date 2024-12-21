namespace Application.Common.Models;

public record PaginatedList<T>
{
    public IReadOnlyList<T> Items { get; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }

    public PaginatedList(IReadOnlyList<T> items, int totalCount, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        Items = items;
    }
}

public record PaginatedListWithHasMore<T>
{
    public IReadOnlyList<T> Items { get; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public bool HasMore { get; }

    public PaginatedListWithHasMore(IReadOnlyList<T> items, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;

        if (items.Count > pageSize)
        {
            HasMore = true;
            Items = items.Take(pageSize).ToList();
        }
        else
        {
            HasMore = false;
            Items = items;
        }
    }
}
