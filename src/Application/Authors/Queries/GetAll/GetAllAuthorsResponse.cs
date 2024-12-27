using Application.Authors.Common.Models;
using Application.Common.Models;

namespace Application.Authors.Queries.GetAll;

public record GetAllAuthorsResponse : PaginatedList<AuthorDto>
{
    public GetAllAuthorsResponse(IReadOnlyList<AuthorDto> items, int totalCount, int pageIndex, int pageSize) : base(items, totalCount, pageIndex, pageSize)
    {
    }
}
