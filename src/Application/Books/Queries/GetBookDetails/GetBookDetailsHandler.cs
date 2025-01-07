using Application.Common.Interfaces;

namespace Application.Books.Queries.GetBookDetails;

public class GetBookDetailsHandler : IRequestHandler<GetBookDetailsQuery, GetBookDetailsResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public GetBookDetailsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetBookDetailsResponse> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
    {
        var borrowedBook = await _dbContext.BorrowedBooksQuery
            .FirstOrDefaultAsync(x => x.BookId == request.BookId && x.UserId == request.UserId && !x.EndDate.HasValue, cancellationToken);

        var book = await _dbContext.BooksQuery
            .Where(x => x.Id == request.BookId)
            .Join(_dbContext.Publishers, x => x.PublisherId, x => x.Id, (book, publisher) => new
            {
                book,
                publisher
            })
            .Join(_dbContext.Categories, x => x.book.CategoryId, x => x.Id, (result, category) => new
            {
                result.book,
                result.publisher,
                category
            })
            .Join(_dbContext.Authors, x => x.book.AuthorId, x => x.Id, (result, auther) => new
            {
                result.book,
                result.publisher,
                result.category,
                auther
            })
            .Select(x => new GetBookDetailsResponse
            {
                Id = x.book.Id,
                Description = x.book.Description,
                CreatedAt = x.book.CreatedAt,
                Interpreters = x.book.Interpreters,
                Language = x.book.Language,
                PublicationDate = x.book.PublicationDate,
                BookInBranches = x.book.BookInBranches,
                Image = x.book.Image,
                Title = x.book.Title,
                Author = new(x.auther.Id, x.auther.FirstName, x.auther.LastName),
                Category = new(x.category.Id, x.category.Title),
                Publisher = new(x.publisher.Id, x.publisher.Name),
                UserBorrowedBook = borrowedBook != null
            })
            .FirstAsync(cancellationToken);

        return book;
    }
}
