using Application.Common.Models;
using Domain.Entities.BookAggregate.Enums;
using Domain.Enums;

namespace Application.Books.Queries.AdminGetBorrowedBooks;

public record AdminGetBorrowedBooksResponse : PaginatedList<AdminGetBorrowedBooksResponse.Item>
{
    public AdminGetBorrowedBooksResponse(IReadOnlyList<Item> items, int totalCount, int pageIndex, int pageSize) : base(items, totalCount, pageIndex, pageSize)
    {
    }

    public required int MaxBorrowDurationDays { get; init; }

    public record Item
    {
        public required string Id { get; init; }
        public required string BookId { get; init; }
        public required string BookTitle { get; init; }
        public required UserDto User { get; init; }
        public string? BookImage { get; init; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required bool DeadlineReached { get; set; }
        public required LibraryBranch Branch { get; set; }
        public required BorrowedBookStatus Status { get; set; }
    }

    public record UserDto(string Id, string FirstName, string LastName);
}
