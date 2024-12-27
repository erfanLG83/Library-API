using Application.Authors.Common.Models;
using Application.Common.Extensions;
using Application.Common.Interfaces;

namespace Application.Authors.Queries.GetAll;

public class GetAllAuthorsHandler : IRequestHandler<GetAllAuthorsQuery, GetAllAuthorsResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllAuthorsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetAllAuthorsResponse> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.AuthorsQuery
            .When(request.SearchTerm is not null, x => (x.FirstName + " " + x.LastName).Contains(request.SearchTerm!));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(x => x.LastName)
            .Skip(request.Skip)
            .Take(request.PageSize)
            .Select(x => new AuthorDto(x.Id, x.FirstName, x.LastName))
            .ToListAsync(cancellationToken);

        return new(items, totalCount, request.PageIndex, request.PageSize);
    }
}
