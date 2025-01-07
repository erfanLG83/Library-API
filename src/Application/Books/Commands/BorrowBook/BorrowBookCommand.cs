using Domain.Enums;

namespace Application.Books.Commands.BorrowBook;

public record BorrowBookCommand(string UserId, string BookId, LibraryBranch Branch) : IRequest<Result>;