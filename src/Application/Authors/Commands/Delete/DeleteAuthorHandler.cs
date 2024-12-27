using Application.Common.Interfaces;

namespace Application.Authors.Commands.Delete;

public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteAuthorHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _dbContext.AuthorsQuery.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (author == null)
        {
            return AuthorErrors.AuthorNotFound;
        }

        var authorHasAnyBook = await _dbContext.BooksQuery.AnyAsync(x => x.AuthorId == author.Id, cancellationToken);
        if (authorHasAnyBook)
        {
            return AuthorErrors.AuthorHasBookAndCanNotDelete;
        }

        await _dbContext.Authors.DeleteOneAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        return Result.Ok();
    }
}
