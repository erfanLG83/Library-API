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
        var items = await _dbContext.AuthorsQuery
            .OrderBy(x => x.LastName)
            .Select(x => new GetAllAuthorsResponse.Item
            {
                FirstName = x.FirstName,
                Id = x.Id,
                LastName = x.LastName,
            })
            .ToListAsync(cancellationToken);

        return new()
        {
            Items = items
        };
    }
}
