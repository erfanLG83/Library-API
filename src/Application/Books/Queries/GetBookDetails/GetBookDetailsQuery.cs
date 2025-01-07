namespace Application.Books.Queries.GetBookDetails;

public record GetBookDetailsQuery(string BookId, string UserId) : IRequest<GetBookDetailsResponse>;
