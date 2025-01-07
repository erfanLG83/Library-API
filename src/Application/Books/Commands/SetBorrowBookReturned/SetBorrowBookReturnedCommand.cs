namespace Application.Books.Commands.SetBorrowBookReturned;

public record SetBorrowBookReturnedCommand(string Id) : IRequest;
