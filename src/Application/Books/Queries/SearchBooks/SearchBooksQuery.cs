using Application.Common.Models;

namespace Application.Books.Queries.SearchBooks;

public record SearchBooksQuery(string? SearchTerm, string? AuthorId, string? CategoryId, string? PublisherId) : PaginationDto, IRequest<SearchBooksResponse>;