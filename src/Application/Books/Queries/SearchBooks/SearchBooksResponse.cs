using Application.Authors.Common.Models;
using Application.Categories.Common.Models;
using Application.Common.Models;
using Domain.Entities.BookAggregate;

namespace Application.Books.Queries.SearchBooks;

public record SearchBooksResponse : PaginatedList<SearchBooksResponse.Item>
{
    public SearchBooksResponse(IReadOnlyList<Item> items, int totalCount, int pageIndex, int pageSize) : base(items, totalCount, pageIndex, pageSize)
    {
    }

    public record Item
    {
        public required string Id { get; init; }
        public required string Title { get; init; }
        public required CategoryDto Category { get; init; }
        public required AuthorDto Author { get; init; }
        public required string? Image { get; set; }
        public required List<BookInBranch> BookInBranches { get; set; }
    }
}
