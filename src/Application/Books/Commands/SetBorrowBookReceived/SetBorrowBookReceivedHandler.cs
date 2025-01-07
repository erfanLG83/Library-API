using Application.Common.Interfaces;
using Domain.Entities.BookAggregate.Enums;

namespace Application.Books.Commands.SetBorrowBookReceived;

public class SetBorrowBookReceivedHandler : IRequestHandler<SetBorrowBookReceivedCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public SetBorrowBookReceivedHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(SetBorrowBookReceivedCommand request, CancellationToken cancellationToken)
    {
        var borrowedBook = await _dbContext.BorrowedBooksQuery
            .FirstAsync(x => x.Id == request.Id, cancellationToken);

        borrowedBook.Status = BorrowedBookStatus.Received;

        await _dbContext.BorrowedBooks.ReplaceOneAsync(x => x.Id == request.Id, borrowedBook, cancellationToken: cancellationToken);
    }
}
