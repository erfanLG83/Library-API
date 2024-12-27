using Domain.Common;

namespace Domain.Entities.PublisherAggregate;

public class Publisher : Entity
{
    public required string Name { get; set; }
}
