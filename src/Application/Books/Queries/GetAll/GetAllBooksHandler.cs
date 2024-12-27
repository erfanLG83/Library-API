﻿using Application.Common.Extensions;
using Application.Common.Interfaces;

namespace Application.Books.Queries.GetAll;

public class GetAllBooksHandler : IRequestHandler<GetAllBooksQuery, GetAllBooksResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllBooksHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetAllBooksResponse> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.BooksQuery
            .ExcludeSoftDelete()
            .When(request.SearchTerm is not null, x => x.Title.Contains(request.SearchTerm!));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(x => x.Title)
            .Skip(request.Skip)
            .Take(request.PageSize)
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
            .Select(x => new GetAllBooksResponse.Item
            {
                Id = x.book.Id,
                Description = x.book.Description,
                Interpreters = x.book.Interpreters,
                Language = x.book.Language,
                PublicationDate = x.book.PublicationDate,
                Quantity = x.book.Quantity,
                Title = x.book.Title,
                Author = new(x.auther.Id, x.auther.FirstName, x.auther.LastName),
                Category = new(x.category.Id, x.category.Title),
                Publisher = new(x.publisher.Id, x.publisher.Name),
            })
            .ToListAsync(cancellationToken);

        return new(items, totalCount, request.PageIndex, request.PageSize);
    }
}
