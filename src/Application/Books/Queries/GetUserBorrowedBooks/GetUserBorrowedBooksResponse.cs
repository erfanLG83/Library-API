using Application.Common.Models;
using Domain.Enums;

namespace Application.Books.Queries.GetUserBorrowedBooks;

public record GetUserBorrowedBooksResponse : PaginatedList<GetUserBorrowedBooksResponse.Item>
{
    public GetUserBorrowedBooksResponse(IReadOnlyList<Item> items, int totalCount, int pageIndex, int pageSize) : base(items, totalCount, pageIndex, pageSize)
    {
    }

    public required int MaxBorrowDurationDays { get; init; }

    public record Item
    {
        public required string Id { get; init; }
        public required string BookId { get; init; }
        public required string BookTitle { get; init; }
        public string? BookImage { get; init; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required bool DeadlineReached { get; set; }
        public required LibraryBranch Branch { get; set; }
    }
}
