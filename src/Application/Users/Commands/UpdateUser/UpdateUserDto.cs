namespace Application.Users.Commands.UpdateUser;

public record UpdateUserDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}