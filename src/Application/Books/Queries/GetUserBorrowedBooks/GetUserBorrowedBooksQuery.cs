using Application.Common.Models;

namespace Application.Books.Queries.GetUserBorrowedBooks;

public record GetUserBorrowedBooksQuery(string UserId, PaginationDto Pagination) : IRequest<GetUserBorrowedBooksResponse>;
