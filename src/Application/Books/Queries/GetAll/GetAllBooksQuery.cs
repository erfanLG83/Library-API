using Application.Books.Common.Models;
using Application.Common.Models;

namespace Application.Books.Queries.GetAll;

public record GetAllBooksQuery(string? SearchTerm,
                               bool IsDescending = false,
                               BooksOrderBy OrderBy = BooksOrderBy.Title) : PaginationDto, IRequest<GetAllBooksResponse>;
