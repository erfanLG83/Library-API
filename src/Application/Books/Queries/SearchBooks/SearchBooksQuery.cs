using Application.Common.Models;
using Domain.Enums;

namespace Application.Books.Queries.SearchBooks;

public record SearchBooksQuery(string? SearchTerm, string? AuthorId, string? CategoryId, string? PublisherId, LibraryBranch? Branch) : PaginationDto, IRequest<SearchBooksResponse>;