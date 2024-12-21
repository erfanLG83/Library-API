using Domain.Common;
using Domain.Entities.UserAggregate;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.BookAggregate;

public class BorrowedBook : Entity
{
    [BsonDateTimeOptions(DateOnly = true, Kind = DateTimeKind.Local, Representation = BsonType.DateTime)]
    public required DateOnly StartDate { get; set; }
    [BsonDateTimeOptions(DateOnly = true, Kind = DateTimeKind.Local, Representation = BsonType.DateTime)]
    public DateOnly? EndDate { get; set; }

    public required string BookId { get; set; }
    public required string UserId { get; set; }

    [BsonIgnore]
    public Book? Book { get; set; }
    [BsonIgnore]
    public User? User { get; set; }
}