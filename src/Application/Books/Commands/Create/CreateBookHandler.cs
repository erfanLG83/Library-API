using Application.Common.Interfaces;
using Domain.Entities.BookAggregate;

namespace Application.Books.Commands.Create;

public class CreateBookHandler : IRequestHandler<CreateBookCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateBookHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            AuthorId = request.AuthorId,
            CategoryId = request.CategoryId,
            Description = request.Description,
            Language = request.Language,
            PublicationDate = request.PublicationDate,
            PublisherId = request.PublisherId,
            Quantity = request.Quantity,
            Title = request.Title,
            Interpreters = request.Interpreters,
        };

        await _dbContext.Books.InsertOneAsync(book, cancellationToken: cancellationToken);
    }
}
