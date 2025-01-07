using Application.Books.Common;
using Application.Books.Common.Models;
using Application.Common.Interfaces;
using Elastic.Clients.Elasticsearch;
using Result = Application.Common.Result;

namespace Application.Books.Commands.Delete;

public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IFileManager _fileManager;
    private readonly ElasticsearchClient _elasticsearch;

    public DeleteBookHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider, ElasticsearchClient elasticsearch, IFileManager fileManager)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _elasticsearch = elasticsearch;
        _fileManager = fileManager;
    }

    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _dbContext.BooksQuery.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (book == null)
        {
            return BooksErrors.BookNotFound;
        }

        book.DeletedAt = _dateTimeProvider.Now;


        var elasticResponse = await _elasticsearch.DeleteByQueryAsync<BookDto>(x =>
            x.Query(q =>
                q.Term(t =>
                    t.Field(f => f.Id).Value(request.Id))), CancellationToken.None);
        if (!elasticResponse.IsValidResponse)
        {
            await _dbContext.Books.DeleteOneAsync(x => x.Id == book.Id, CancellationToken.None);
            throw new Exception("Failed to delete book in elastic");
        }

        if (book.Image != null)
        {
            _fileManager.Delete(book.Image);
        }

        await _dbContext.Books.ReplaceOneAsync(x => x.Id == request.Id, book, cancellationToken: cancellationToken);

        return Result.Ok();
    }
}
