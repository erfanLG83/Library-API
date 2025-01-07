using Application.Books.Common.Models;

namespace Application.Books.Queries.GetBookDetails;

public record GetBookDetailsResponse : BookDto
{
    public bool UserBorrowedBook { get; set; }
}
