using Domain.Common;
using Domain.Entities.UserAggregate;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.BookAggregate;

public class BorrowedBook : Entity
{
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public required string BookId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public required string UserId { get; set; }

    [BsonIgnore]
    public Book? Book { get; set; }
    [BsonIgnore]
    public User? User { get; set; }
}