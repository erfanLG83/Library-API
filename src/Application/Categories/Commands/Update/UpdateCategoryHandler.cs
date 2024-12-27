using Application.Common.Interfaces;

namespace Application.Categories.Commands.Update;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateCategoryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.CategoriesQuery.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (category == null)
        {
            return CategoriesErrors.CategoryNotFound;
        }

        category.Title = request.Title;

        await _dbContext.Categories.ReplaceOneAsync(x => x.Id == request.Id, category, cancellationToken: cancellationToken);

        return Result.Ok();
    }
}
