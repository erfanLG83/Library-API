using Domain.Common;
using Domain.Entities.BookAggregate;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.AuthorAggregate;

public class Author : Entity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    [BsonIgnore]
    public List<Book>? Books { get; set; }
}
