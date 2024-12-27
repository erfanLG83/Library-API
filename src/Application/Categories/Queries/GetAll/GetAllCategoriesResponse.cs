using Application.Common.Models;

namespace Application.Categories.Queries.GetAll;

public record GetAllCategoriesResponse : PaginatedList<GetAllCategoriesResponse.Item>
{
    public GetAllCategoriesResponse(IReadOnlyList<Item> items, int totalCount, int pageIndex, int pageSize) : base(items, totalCount, pageIndex, pageSize)
    {
    }

    public record Item
    {
        public required string Id { get; init; }
        public required string Title { get; init; }
    }
}
