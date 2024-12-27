using Domain.Common;
using Domain.Entities.AuthorAggregate;
using Domain.Entities.CategoryAggregate;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.BookAggregate;

public class Book : Entity
{
    public required string Title { get; set; }
    public required int PublicationYear { get; set; }
    public required string AuthorId { get; set; }
    public required string CategoryId { get; set; }

    [BsonIgnore]
    public Author? Author { get; set; }
    [BsonIgnore]
    public Category? Category { get; set; }
    [BsonIgnore]
    public List<BorrowedBook>? BorrowedBooks { get; set; }
}