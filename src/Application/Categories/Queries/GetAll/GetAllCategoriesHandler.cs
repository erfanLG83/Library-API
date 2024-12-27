using Application.Common.Extensions;
using Application.Common.Interfaces;

namespace Application.Categories.Queries.GetAll;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, GetAllCategoriesResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllCategoriesHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetAllCategoriesResponse> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.CategoriesQuery
            .When(request.SearchTerm is not null, x => x.Title.Contains(request.SearchTerm!));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(x => x.Title)
            .Skip(request.Skip)
            .Take(request.PageSize)
            .Select(x => new GetAllCategoriesResponse.Item
            {
                Id = x.Id,
                Title = x.Title
            })
            .ToListAsync(cancellationToken);

        return new(items, totalCount, request.PageIndex, request.PageSize);
    }
}
