using Domain.Entities.UserAggregate.Enums;

namespace Application.Users.Queries.GetUserInfo;

public record GetUserInfoResponse
{
    public required string Id { get; init; }
    public required string LastName { get; init; }
    public required string FirstName { get; init; }
    public required string PhoneNumber { get; init; }
    public required UserRole Role { get; init; }
}
