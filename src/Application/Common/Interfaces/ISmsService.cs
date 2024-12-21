namespace Application.Common.Interfaces;

public interface ISmsService
{
    Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken);
    Task SendOtpCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken);
}