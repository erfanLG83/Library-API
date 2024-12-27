using Domain.Entities.BookAggregate.Enums;

namespace Application.Books.Commands.Create;

public record CreateBookCommand : IRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string PublicationDate { get; set; }
    public required BookLanguage Language { get; set; }
    public required int Quantity { get; set; }
    public string? Interpreters { get; set; }

    public required string AuthorId { get; set; }
    public required string PublisherId { get; set; }
    public required string CategoryId { get; set; }
}
