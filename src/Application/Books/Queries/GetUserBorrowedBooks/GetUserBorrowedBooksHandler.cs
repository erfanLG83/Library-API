using Application.Common.Interfaces;

namespace Application.Books.Queries.GetUserBorrowedBooks;

public class GetUserBorrowedBooksHandler : IRequestHandler<GetUserBorrowedBooksQuery, GetUserBorrowedBooksResponse>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private static readonly int MaxBorrowDurationDays = 30;

    public GetUserBorrowedBooksHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<GetUserBorrowedBooksResponse> Handle(GetUserBorrowedBooksQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.BorrowedBooksQuery
            .Where(x => x.UserId == request.UserId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.OrderByDescending(x => x.StartDate)
            .Skip(request.Pagination.Skip)
            .Take(request.Pagination.PageSize)
            .Join(_dbContext.BooksQuery, x => x.BookId, x => x.Id, (borrow, book) => new
            {
                borrow,
                book
            })
            .Select(x => new GetUserBorrowedBooksResponse.Item
            {
                BookId = x.book.Id,
                BookImage = x.book.Image,
                BookTitle = x.book.Title,
                Id = x.borrow.Id,
                StartDate = x.borrow.StartDate,
                EndDate = x.borrow.EndDate,
                Branch = x.borrow.Branch,
                DeadlineReached = !x.borrow.EndDate.HasValue && x.borrow.StartDate.AddDays(MaxBorrowDurationDays) < _dateTimeProvider.Now
            }).ToListAsync(cancellationToken);

        return new(items, totalCount, request.Pagination.PageIndex, request.Pagination.PageSize)
        {
            MaxBorrowDurationDays = MaxBorrowDurationDays
        };
    }
}
