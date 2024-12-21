using Application.Users.Common.DTOs;

namespace Application.Users.Commands.VerifyPhoneNumber;

public record VerifyPhoneNumberCommand : IRequest<Result<UserTokenDto>>
{
    public required string PhoneNumber { get; init; }
    public required string OtpCode { get; init; }
    public string? FirstName { get; set; }
    public string? LastName { get; init; }
}