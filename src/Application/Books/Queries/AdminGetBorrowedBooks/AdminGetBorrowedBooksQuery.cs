using Application.Common.Models;

namespace Application.Books.Queries.AdminGetBorrowedBooks;

public record AdminGetBorrowedBooksQuery(PaginationDto Pagination) : IRequest<AdminGetBorrowedBooksResponse>;
