using Application.Common.Interfaces;

namespace Application.Books.Queries.AdminGetBorrowedBooks;

public class AdminGetBorrowedBooksHandler : IRequestHandler<AdminGetBorrowedBooksQuery, AdminGetBorrowedBooksResponse>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private static readonly int MaxBorrowDurationDays = 30;

    public AdminGetBorrowedBooksHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<AdminGetBorrowedBooksResponse> Handle(AdminGetBorrowedBooksQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.BorrowedBooksQuery;

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.OrderByDescending(x => x.StartDate)
            .Skip(request.Pagination.Skip)
            .Take(request.Pagination.PageSize)
            .Join(_dbContext.BooksQuery, x => x.BookId, x => x.Id, (borrow, book) => new
            {
                borrow,
                book
            })
            .Join(_dbContext.UsersQuery, x => x.borrow.UserId, x => x.Id, (current, user) => new
            {
                current.borrow,
                current.book,
                user
            })
            .Select(x => new AdminGetBorrowedBooksResponse.Item
            {
                User = new(x.user.Id, x.user.FirstName, x.user.LastName),
                BookId = x.book.Id,
                BookImage = x.book.Image,
                BookTitle = x.book.Title,
                Id = x.borrow.Id,
                StartDate = x.borrow.StartDate,
                EndDate = x.borrow.EndDate,
                Branch = x.borrow.Branch,
                Status = x.borrow.Status,
                DeadlineReached = !x.borrow.EndDate.HasValue && x.borrow.StartDate.AddDays(MaxBorrowDurationDays) < _dateTimeProvider.Now
            }).ToListAsync(cancellationToken);

        return new(items, totalCount, request.Pagination.PageIndex, request.Pagination.PageSize)
        {
            MaxBorrowDurationDays = MaxBorrowDurationDays
        };
    }
}
