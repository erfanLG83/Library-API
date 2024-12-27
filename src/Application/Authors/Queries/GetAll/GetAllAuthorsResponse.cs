using Application.Common.Models;

namespace Application.Authors.Queries.GetAll;

public record GetAllAuthorsResponse : PaginatedList<GetAllAuthorsResponse.Item>
{
    public GetAllAuthorsResponse(IReadOnlyList<Item> items, int totalCount, int pageIndex, int pageSize) : base(items, totalCount, pageIndex, pageSize)
    {
    }

    public record Item
    {
        public required string Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
    }
}
