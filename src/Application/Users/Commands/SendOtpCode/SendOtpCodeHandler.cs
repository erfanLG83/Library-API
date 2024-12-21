using Application.Common.Interfaces;
using Application.Common.Utilities;
using Application.Users.Common;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Users.Commands.SendOtpCode;

public class SendOtpCodeHandler : IRequestHandler<SendOtpCodeCommand, Result<SendOtpCodeResponse>>
{
    private static readonly TimeSpan _otpCodeTtl = TimeSpan.FromMinutes(2);

    private readonly IApplicationDbContext _dbContext;
    private readonly ISmsService _smsService;
    private readonly IMemoryCache _memoryCache;

    public SendOtpCodeHandler(IApplicationDbContext dbContext, ISmsService smsService, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _smsService = smsService;
        _memoryCache = memoryCache;
    }

    public async Task<Result<SendOtpCodeResponse>> Handle(SendOtpCodeCommand request, CancellationToken cancellationToken)
    {
        var phoneNumberExists = await _dbContext.UsersQuery.AnyAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);

        var cacheKey = $"otp_{request.PhoneNumber}";

        if (_memoryCache.TryGetValue(cacheKey, out _))
        {
            return UserErrors.CanNotResendOtpCode;
        }

        var otpCode = OTPCodeGenerator.GenerateCode();
        await _smsService.SendOtpCodeAsync(request.PhoneNumber, otpCode, cancellationToken);

        _memoryCache.Set(cacheKey, otpCode);

        return new SendOtpCodeResponse(_otpCodeTtl, !phoneNumberExists);
    }
}