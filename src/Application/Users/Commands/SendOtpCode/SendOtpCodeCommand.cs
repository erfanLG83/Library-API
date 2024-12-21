namespace Application.Users.Commands.SendOtpCode;

public record SendOtpCodeCommand : IRequest<Result<SendOtpCodeResponse>>
{
    public required string PhoneNumber { get; set; }
}