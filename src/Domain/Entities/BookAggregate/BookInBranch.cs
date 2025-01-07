using Domain.Enums;

namespace Domain.Entities.BookAggregate;

public class BookInBranch
{
    public required LibraryBranch Branch { get; init; }
    public required int Quantity { get; set; }
}
