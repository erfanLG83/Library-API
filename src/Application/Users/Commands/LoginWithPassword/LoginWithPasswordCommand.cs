using Application.Users.Common.DTOs;

namespace Application.Users.Commands.LoginWithPassword;

public record LoginWithPasswordCommand : IRequest<Result<UserTokenDto>>
{
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
}