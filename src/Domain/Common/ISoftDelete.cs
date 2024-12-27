using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Common;

public interface ISoftDelete
{
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime? DeletedAt { get; set; }
}