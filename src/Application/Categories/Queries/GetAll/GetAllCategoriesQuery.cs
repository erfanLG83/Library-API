using Application.Common.Models;

namespace Application.Categories.Queries.GetAll;

public record GetAllCategoriesQuery(string? SearchTerm) : PaginationDto, IRequest<GetAllCategoriesResponse>;
