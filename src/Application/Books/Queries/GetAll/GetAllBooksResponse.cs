using Application.Books.Common.Models;
using Application.Common.Models;

namespace Application.Books.Queries.GetAll;

public record GetAllBooksResponse : PaginatedList<BookDto>
{
    public GetAllBooksResponse(IReadOnlyList<BookDto> items, int totalCount, int pageIndex, int pageSize) : base(items, totalCount, pageIndex, pageSize)
    {
    }
}
