using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Publishers.Common.Models;

namespace Application.Publishers.Queries.GetAll;

public class GetAllPublishersHandler : IRequestHandler<GetAllPublishersQuery, GetAllPublishersResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllPublishersHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetAllPublishersResponse> Handle(GetAllPublishersQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.PublishersQuery
            .When(request.SearchTerm is not null, x => x.Name.Contains(request.SearchTerm!));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(x => x.Name)
            .Skip(request.Skip)
            .Take(request.PageSize)
            .Select(x => new PublisherDto(x.Id, x.Name))
            .ToListAsync(cancellationToken);

        return new(items, totalCount, request.PageIndex, request.PageSize);
    }
}
