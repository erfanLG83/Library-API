using Application.Authors;
using Application.Common.Interfaces;

namespace Application.Books.Commands.Delete;

public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeleteBookHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _dbContext.BooksQuery.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (book == null)
        {
            return BooksErrors.BookNotFound;
        }

        book.DeletedAt = _dateTimeProvider.Now;

        await _dbContext.Books.ReplaceOneAsync(x => x.Id == request.Id, book, cancellationToken: cancellationToken);

        return Result.Ok();
    }
}
