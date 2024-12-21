using Application.Common.Interfaces;
using Domain.Entities.AuthorAggregate;

namespace Application.Authors.Commands.Create;

public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateAuthorHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = new Author
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
        };

        await _dbContext.Authors.InsertOneAsync(author, cancellationToken: cancellationToken);
    }
}
