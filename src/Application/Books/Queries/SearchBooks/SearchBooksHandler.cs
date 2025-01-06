using Application.Books.Common.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System.Linq.Expressions;

namespace Application.Books.Queries.SearchBooks;

public class SearchBooksHandler : IRequestHandler<SearchBooksQuery, SearchBooksResponse>
{
    private readonly ElasticsearchClient _elasticsearch;

    public SearchBooksHandler(ElasticsearchClient elasticsearch)
    {
        _elasticsearch = elasticsearch;
    }

    public async Task<SearchBooksResponse> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
    {
        var searchTermFields = new Expression<Func<BookDto, object?>>[]
        {
            x => x.Publisher.Name,
            x => x.Author.FirstName,
            x => x.Author.LastName,
            x => x.Description,
            x => x.Title,
            x => x.Category.Title
        };

        var filters = new List<Action<QueryDescriptor<BookDto>>>();

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            filters.Add(q => q.MultiMatch(m => m.Type(TextQueryType.BestFields)
                .Fields(Fields.FromExpressions(searchTermFields))
                .Query(request.SearchTerm)
            ));
        }

        if (!string.IsNullOrEmpty(request.AuthorId))
        {
            filters.Add(q => q.Term(t => t
                .Field(b => b.Author.Id)
                .Value(request.AuthorId)
            ));
        }

        if (!string.IsNullOrEmpty(request.CategoryId))
        {
            filters.Add(q => q.Term(t => t
                .Field(b => b.Category.Id)
                .Value(request.CategoryId)
            ));
        }

        if (!string.IsNullOrEmpty(request.PublisherId))
        {
            filters.Add(q => q.Term(t => t
                .Field(b => b.Publisher.Id)
                .Value(request.PublisherId)
            ));
        }

        var searchResponse = await _elasticsearch.SearchAsync<BookDto>(s => s
            .From(request.Skip)
            .Size(request.PageSize)
            .Query(q => q.Bool(x => x.Must(filters.ToArray())))
        , cancellationToken);

        if (!searchResponse.IsValidResponse)
        {
            throw new Exception($"Error executing search: {searchResponse.ElasticsearchServerError?.Error?.Reason}");
        }

        var items = searchResponse.Documents.Select(x => new SearchBooksResponse.Item
        {
            Author = x.Author,
            Category = x.Category,
            Id = x.Id,
            Title = x.Title
        }).ToList();

        return new SearchBooksResponse(items, (int)searchResponse.Total, request.PageIndex, request.PageSize);
    }
}
