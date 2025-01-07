using Application.Common.Interfaces;
using Domain.Entities.BookAggregate.Enums;

namespace Application.Books.Commands.SetBorrowBookReturned;

public class SetBorrowBookReturnedHandler : IRequestHandler<SetBorrowBookReturnedCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SetBorrowBookReturnedHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(SetBorrowBookReturnedCommand request, CancellationToken cancellationToken)
    {
        var borrowedBook = await _dbContext.BorrowedBooksQuery
            .FirstAsync(x => x.Id == request.Id, cancellationToken);
        var book = await _dbContext.BooksQuery.FirstAsync(x => x.Id == borrowedBook.BookId, cancellationToken);

        borrowedBook.Status = BorrowedBookStatus.Returned;
        borrowedBook.EndDate = _dateTimeProvider.Now;

        await _dbContext.BorrowedBooks.ReplaceOneAsync(x => x.Id == request.Id, borrowedBook, cancellationToken: cancellationToken);

        book.BookInBranches.First(x => x.Branch == borrowedBook.Branch).Quantity++;

        await _dbContext.Books.ReplaceOneAsync(x => x.Id == book.Id, book, cancellationToken: cancellationToken);
    }
}
