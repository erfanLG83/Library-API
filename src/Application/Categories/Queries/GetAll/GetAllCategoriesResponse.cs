using Application.Categories.Common.Models;
using Application.Common.Models;

namespace Application.Categories.Queries.GetAll;

public record GetAllCategoriesResponse : PaginatedList<CategoryDto>
{
    public GetAllCategoriesResponse(IReadOnlyList<CategoryDto> items, int totalCount, int pageIndex, int pageSize) : base(items, totalCount, pageIndex, pageSize)
    {
    }
}
