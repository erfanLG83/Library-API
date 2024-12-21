namespace Application.Users.Commands.SendOtpCode;

public record SendOtpCodeResponse(TimeSpan OtpCodeTtl, bool isNewUser);