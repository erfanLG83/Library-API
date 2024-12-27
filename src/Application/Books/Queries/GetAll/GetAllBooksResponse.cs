using Application.Authors.Common.Models;
using Application.Categories.Common.Models;
using Application.Common.Models;
using Application.Publishers.Common.Models;
using Domain.Entities.BookAggregate.Enums;

namespace Application.Books.Queries.GetAll;

public record GetAllBooksResponse : PaginatedList<GetAllBooksResponse.Item>
{
    public GetAllBooksResponse(IReadOnlyList<Item> items, int totalCount, int pageIndex, int pageSize) : base(items, totalCount, pageIndex, pageSize)
    {
    }

    public record Item
    {
        public required string Id { get; init; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string? Interpreters { get; set; }
        public required string PublicationDate { get; set; }
        public required BookLanguage Language { get; set; }
        public required int Quantity { get; set; }
        public required CategoryDto Category { get; set; }
        public required AuthorDto Author { get; set; }
        public required PublisherDto Publisher { get; set; }
    }
}
