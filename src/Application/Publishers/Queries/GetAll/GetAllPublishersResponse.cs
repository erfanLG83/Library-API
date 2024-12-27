using Application.Common.Models;
using Application.Publishers.Common.Models;

namespace Application.Publishers.Queries.GetAll;

public record GetAllPublishersResponse : PaginatedList<PublisherDto>
{
    public GetAllPublishersResponse(IReadOnlyList<PublisherDto> items, int totalCount, int pageIndex, int pageSize) : base(items, totalCount, pageIndex, pageSize)
    {
    }
}
