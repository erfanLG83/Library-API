using Domain.Entities.UserAggregate;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public IMongoCollection<User> Users { get; }
    public IQueryable<User> UsersQuery { get; }
}