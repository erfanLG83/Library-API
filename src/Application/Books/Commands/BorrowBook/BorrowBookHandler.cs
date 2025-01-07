using Application.Books.Common;
using Application.Common.Interfaces;

namespace Application.Books.Commands.BorrowBook;

public class BorrowBookHandler : IRequestHandler<BorrowBookCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public BorrowBookHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        var borrowedBook = await _dbContext.BorrowedBooksQuery
            .FirstOrDefaultAsync(x => x.BookId == request.BookId && x.UserId == request.UserId && !x.EndDate.HasValue, cancellationToken);

        var book = await _dbContext.BooksQuery.FirstAsync(x => x.Id == request.BookId, cancellationToken);

        if (borrowedBook is not null)
        {
            return BooksErrors.UserAlreadyBorrowedBook;
        }

        var bookInBranch = book.BookInBranches.First(x => x.Branch == request.Branch);

        if (bookInBranch.Quantity <= 0)
        {
            return BooksErrors.BookIsNotAvailable;
        }

        borrowedBook = new()
        {
            BookId = request.BookId,
            StartDate = _dateTimeProvider.Now,
            UserId = request.UserId,
        };

        await _dbContext.BorrowedBooks.InsertOneAsync(borrowedBook, cancellationToken: cancellationToken);

        bookInBranch.Quantity--;

        await _dbContext.Books.ReplaceOneAsync(x => x.Id == borrowedBook.Id, book, cancellationToken: cancellationToken);

        return Result.Ok();
    }
}
