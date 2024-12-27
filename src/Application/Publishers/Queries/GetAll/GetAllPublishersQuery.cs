using Application.Common.Models;

namespace Application.Publishers.Queries.GetAll;

public record GetAllPublishersQuery(string? SearchTerm) : PaginationDto, IRequest<GetAllPublishersResponse>;
