using Application.Common.Interfaces;

namespace Infrastructure.Services;

public class TestSmsService : ISmsService
{
    public Task SendOtpCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(phoneNumber, nameof(phoneNumber));
        ArgumentException.ThrowIfNullOrEmpty(code, nameof(code));
        return Task.CompletedTask;
    }

    public Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(phoneNumber,nameof(phoneNumber));
        ArgumentException.ThrowIfNullOrEmpty(message, nameof(message));
        return Task.CompletedTask;
    }
}
