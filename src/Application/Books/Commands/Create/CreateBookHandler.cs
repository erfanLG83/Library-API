using Application.Books.Common.Models;
using Application.Common.Interfaces;
using Domain.Entities.BookAggregate;
using Elastic.Clients.Elasticsearch;

namespace Application.Books.Commands.Create;

public class CreateBookHandler : IRequestHandler<CreateBookCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ElasticsearchClient _elasticsearch;
    private readonly IFileManager _fileManager;

    public CreateBookHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider, ElasticsearchClient elasticsearch, IFileManager fileManager)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _elasticsearch = elasticsearch;
        _fileManager = fileManager;
    }

    public async Task Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var author = await _dbContext.AuthorsQuery.FirstAsync(x => x.Id == request.AuthorId, cancellationToken);
        var category = await _dbContext.CategoriesQuery.FirstAsync(x => x.Id == request.CategoryId, cancellationToken);
        var publisher = await _dbContext.PublishersQuery.FirstAsync(x => x.Id == request.PublisherId, cancellationToken);

        var book = new Book
        {
            CreatedAt = _dateTimeProvider.Now,
            AuthorId = request.AuthorId,
            CategoryId = request.CategoryId,
            Description = request.Description,
            Language = request.Language,
            PublicationDate = request.PublicationDate,
            PublisherId = request.PublisherId,
            BookInBranches = request.BookInBranches,
            Title = request.Title,
            Interpreters = request.Interpreters,
        };

        if (request.Image != null)
        {
            book.Image = await _fileManager.SaveFileAsync(request.Image, IFileManager.Folders.Books, cancellationToken);
        }

        await _dbContext.Books.InsertOneAsync(book, cancellationToken: cancellationToken);

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

        var response = await _elasticsearch.CreateAsync(bookDto, CancellationToken.None);
        if (!response.IsValidResponse)
        {
            await _dbContext.Books.DeleteOneAsync(x => x.Id == book.Id, CancellationToken.None);
            throw new Exception("Failed to create book in elastic");
        }

    }
}
