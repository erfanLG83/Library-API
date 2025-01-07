using Application.Books.Common;
using Application.Books.Common.Models;
using Application.Common.Interfaces;
using Elastic.Clients.Elasticsearch;
using Result = Application.Common.Result;

namespace Application.Books.Commands.Update;

public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IFileManager _fileManager;
    private readonly ElasticsearchClient _elasticsearch;

    public UpdateBookHandler(IApplicationDbContext dbContext, ElasticsearchClient elasticsearch, IFileManager fileManager)
    {
        _dbContext = dbContext;
        _elasticsearch = elasticsearch;
        _fileManager = fileManager;
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
        book.BookInBranches = request.BookInBranches;
        book.Title = request.Title;

        if (request.NewImage != null)
        {
            if (book.Image != null)
            {
                _fileManager.Delete(book.Image);
            }

            book.Image = await _fileManager.SaveFileAsync(request.NewImage, IFileManager.Folders.Books, cancellationToken);
        }


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
            Title = book.Title,
            Image = book.Image,
            BookInBranches = book.BookInBranches,
        };

        var elasticResponse = await _elasticsearch.UpdateAsync<BookDto, BookDto>(new Id(book.Id), x => x.Doc(bookDto), CancellationToken.None);
        if (!elasticResponse.IsValidResponse)
        {
            throw new Exception("Failed to update book in elastic");
        }

        await _dbContext.Books.ReplaceOneAsync(x => x.Id == request.Id, book, cancellationToken: cancellationToken);

        return Result.Ok();
    }
}
