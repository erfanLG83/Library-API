using MongoDB.Bson;

namespace Application.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest<Result>
{
    public required string Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
