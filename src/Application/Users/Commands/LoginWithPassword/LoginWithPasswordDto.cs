namespace Application.Users.Commands.LoginWithPassword;

public record LoginWithPasswordDto
{
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
}
