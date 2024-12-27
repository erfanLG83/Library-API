using Application.Common.Models;

namespace Application.Books.Queries.GetAll;

public record GetAllBooksQuery(string? SearchTerm) : PaginationDto, IRequest<GetAllBooksResponse>;
