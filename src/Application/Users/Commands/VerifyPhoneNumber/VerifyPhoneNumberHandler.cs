using Application.Common.Interfaces;
using Application.Users.Common;
using Application.Users.Common.DTOs;
using Domain.Entities.UserAggregate;
using Domain.Entities.UserAggregate.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace Application.Users.Commands.VerifyPhoneNumber;

public class VerifyPhoneNumberHandler : IRequestHandler<VerifyPhoneNumberCommand, Result<UserTokenDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IMemoryCache _memoryCache;
    private readonly ITokenFactoryService _tokenFactoryService;

    public VerifyPhoneNumberHandler(IApplicationDbContext dbContext,
                                    IHostEnvironment hostEnvironment,
                                    IMemoryCache memoryCache,
                                    ITokenFactoryService tokenFactoryService)
    {
        _dbContext = dbContext;
        _hostEnvironment = hostEnvironment;
        _memoryCache = memoryCache;
        _tokenFactoryService = tokenFactoryService;
    }

    public async Task<Result<UserTokenDto>> Handle(VerifyPhoneNumberCommand request, CancellationToken cancellationToken)
    {
        var validOtpCode = _memoryCache.Get<string>($"otp_{request.PhoneNumber}");
        if (validOtpCode is null)
        {
            return UserErrors.OtpCodeHasExpired;
        }

        var isOtpValid = validOtpCode == request.OtpCode;

        if (isOtpValid == false)
        {
            const string defaultOtpCode = "12345";
            isOtpValid = _hostEnvironment.IsProduction() == false && request.OtpCode == defaultOtpCode;
        }

        if (isOtpValid == false)
            return UserErrors.OtpCodeIsInvalid;

        var user = await _dbContext.UsersQuery.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (user == null)
        {
            if (request.FirstName is null || request.LastName is null)
            {
                return UserErrors.FirstNameAndLastNameIsRequired;
            }

            user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Role = UserRole.Customer
            };

            await _dbContext.Users.InsertOneAsync(user, cancellationToken: cancellationToken);
        }

        var accessToken = _tokenFactoryService.CreateJwt(user);

        return new UserTokenDto(accessToken);
    }
}