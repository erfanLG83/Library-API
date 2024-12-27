using Application.Authors;
using Application.Common.Interfaces;

namespace Application.Books.Commands.Update;

public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateBookHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _dbContext.BooksQuery.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (book == null)
        {
            return BooksErrors.BookNotFound;
        }

        book.AuthorId = request.AuthorId;
        book.PublisherId = request.PublisherId;
        book.CategoryId = request.CategoryId;
        book.Description = request.Description;
        book.Interpreters = request.Interpreters;
        book.Language = request.Language;
        book.PublicationDate = request.PublicationDate;
        book.Quantity = request.Quantity;
        book.Title = request.Title;

        await _dbContext.Books.ReplaceOneAsync(x => x.Id == request.Id, book, cancellationToken: cancellationToken);

        return Result.Ok();
    }
}
