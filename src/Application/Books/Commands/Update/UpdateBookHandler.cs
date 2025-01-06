using Application.Books.Common;
using Application.Books.Common.Models;
using Application.Common.Interfaces;
using Elastic.Clients.Elasticsearch;
using Result = Application.Common.Result;

namespace Application.Books.Commands.Update;

public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ElasticsearchClient _elasticsearch;

    public UpdateBookHandler(IApplicationDbContext dbContext, ElasticsearchClient elasticsearch)
    {
        _dbContext = dbContext;
        _elasticsearch = elasticsearch;
    }

    public async Task<Result> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var author = await _dbContext.AuthorsQuery.FirstAsync(x => x.Id == request.AuthorId, cancellationToken);
        var category = await _dbContext.CategoriesQuery.FirstAsync(x => x.Id == request.CategoryId, cancellationToken);
        var publisher = await _dbContext.PublishersQuery.FirstAsync(x => x.Id == request.PublisherId, cancellationToken);
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


        var bookDto = new BookDto
        {
            Author = new(author.Id, author.FirstName, author.LastName),
            Category = new(category.Id, category.Title),
            Publisher = new(publisher.Id, publisher.Name),
            CreatedAt = book.CreatedAt,
            Description = book.Description,
            Id = book.Id,
            Interpreters = book.Interpreters,
            Language = book.Language,
            PublicationDate = book.PublicationDate,
            Quantity = book.Quantity,
            Title = book.Title
        };

        var elasticResponse = await _elasticsearch.UpdateAsync<BookDto, BookDto>(bookDto, Id.From(bookDto), CancellationToken.None);
        if (!elasticResponse.IsValidResponse)
        {
            throw new Exception("Failed to update book in elastic");
        }

        await _dbContext.Books.ReplaceOneAsync(x => x.Id == request.Id, book, cancellationToken: cancellationToken);

        return Result.Ok();
    }
}
