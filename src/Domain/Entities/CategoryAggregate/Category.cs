using Domain.Common;

namespace Domain.Entities.CategoryAggregate;
public class Category : Entity
{
    public required string Title { get; set; }
}