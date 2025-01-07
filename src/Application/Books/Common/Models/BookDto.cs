using Application.Authors.Common.Models;
using Application.Categories.Common.Models;
using Application.Publishers.Common.Models;
using Domain.Entities.BookAggregate;
using Domain.Entities.BookAggregate.Enums;

namespace Application.Books.Common.Models;

public class BookDto
{
    public required string Id { get; init; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; }
    public string? Interpreters { get; set; }
    public required string PublicationDate { get; set; }
    public required BookLanguage Language { get; set; }
    public string? Image { get; set; }
    public List<BookInBranch> BookInBranches { get; set; } = [];
    public int Branch1Quantity => BookInBranches[0].Quantity;
    public int Branch2Quantity => BookInBranches[1].Quantity;
    public required CategoryDto Category { get; set; }
    public required AuthorDto Author { get; set; }
    public required PublisherDto Publisher { get; set; }
}
