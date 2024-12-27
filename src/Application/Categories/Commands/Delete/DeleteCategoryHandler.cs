using Application.Categories.Common;
using Application.Common.Interfaces;

namespace Application.Categories.Commands.Delete;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteCategoryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.CategoriesQuery.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (category == null)
        {
            return CategoriesErrors.CategoryNotFound;
        }

        var categoryHasAnyBook = await _dbContext.BooksQuery.AnyAsync(x => x.CategoryId == category.Id, cancellationToken);
        if (categoryHasAnyBook)
        {
            return CategoriesErrors.CategoryHasBookAndCanNotDelete;
        }

        await _dbContext.Categories.DeleteOneAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        return Result.Ok();
    }
}
