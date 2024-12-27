using Domain.Common;
using Domain.Entities.AuthorAggregate;
using Domain.Entities.BookAggregate.Enums;
using Domain.Entities.CategoryAggregate;
using Domain.Entities.PublisherAggregate;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.BookAggregate;

public class Book : Entity, ISoftDelete
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? Interpreters { get; set; }
    public required string PublicationDate { get; set; }
    public required BookLanguage Language { get; set; }
    public required int Quantity { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public required string AuthorId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public required string PublisherId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public required string CategoryId { get; set; }

    [BsonIgnore]
    public Author? Author { get; set; }
    [BsonIgnore]
    public Category? Category { get; set; }
    [BsonIgnore]
    public Publisher? Publisher { get; set; }
    [BsonIgnore]
    public List<BorrowedBook>? BorrowedBooks { get; set; }
    public DateTime? DeletedAt { get; set; }
}