using Application.Common.Interfaces;
using Domain.Entities.BookAggregate;

namespace Application.Books.Commands.Create;

public class CreateBookHandler : IRequestHandler<CreateBookCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateBookHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            CreatedAt = _dateTimeProvider.Now,
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
