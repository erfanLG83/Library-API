using Application.Common.Models;

namespace Application.Authors.Queries.GetAll;

public record GetAllAuthorsQuery(string? SearchTerm) : PaginationDto, IRequest<GetAllAuthorsResponse>;
