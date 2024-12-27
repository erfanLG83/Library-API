namespace Application.Books.Commands.Delete;

public class DeleteBookCommand : IRequest<Result>
{
    public required string Id { get; set; }
}
