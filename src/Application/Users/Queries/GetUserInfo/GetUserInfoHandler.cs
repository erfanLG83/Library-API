using Application.Common.Interfaces;

namespace Application.Users.Queries.GetUserInfo;

public class GetUserInfoHandler : IRequestHandler<GetUserInfoQuery, GetUserInfoResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public GetUserInfoHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<GetUserInfoResponse> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        return _dbContext.UsersQuery
            .Where(x => x.Id == request.UserId)
            .Select(x => new GetUserInfoResponse
            {
                Id = x.Id,
                FirstName = x.FirstName!,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                Role = x.Role
            })
            .FirstAsync(cancellationToken);
    }
}
