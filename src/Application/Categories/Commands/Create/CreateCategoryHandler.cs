using Application.Common.Interfaces;
using Domain.Entities.CategoryAggregate;

namespace Application.Categories.Commands.Create;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateCategoryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Title = request.Title,
        };

        await _dbContext.Categories.InsertOneAsync(category, cancellationToken: cancellationToken);
    }
}
