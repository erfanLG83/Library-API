using Domain.Common;
using Domain.Entities.BookAggregate.Enums;
using Domain.Entities.UserAggregate;
using Domain.Enums;
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
    public required LibraryBranch Branch { get; set; }
    public BorrowedBookStatus Status { get; set; } = BorrowedBookStatus.NotReceived;

    [BsonIgnore]
    public Book? Book { get; set; }
    [BsonIgnore]
    public User? User { get; set; }
}