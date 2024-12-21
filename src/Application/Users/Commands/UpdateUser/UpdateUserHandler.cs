using Application.Common.Interfaces;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateUserHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.UsersQuery.FirstAsync(x => x.Id == request.Id, cancellationToken);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        await _dbContext.Users.ReplaceOneAsync(x => x.Id == request.Id, user, cancellationToken: cancellationToken);

        return Result.Ok();
    }
}