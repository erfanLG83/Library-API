namespace Application.Authors.Queries.GetAll;

public record GetAllAuthorsResponse
{
    public required List<Item> Items { get; init; }
    public record Item
    {
        public required string Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
    }
}
