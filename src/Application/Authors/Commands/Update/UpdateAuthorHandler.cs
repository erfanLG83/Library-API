using Application.Authors.Common;
using Application.Common.Interfaces;

namespace Application.Authors.Commands.Update;

public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateAuthorHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _dbContext.AuthorsQuery.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (author == null)
        {
            return AuthorErrors.AuthorNotFound;
        }

        author.FirstName = request.FirstName;
        author.LastName = request.LastName;

        await _dbContext.Authors.ReplaceOneAsync(x => x.Id == request.Id, author, cancellationToken: cancellationToken);

        return Result.Ok();
    }
}
