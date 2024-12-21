using Application.Common.Interfaces;
using Application.Common.Utilities;
using Application.Users.Common;
using Application.Users.Common.DTOs;
using Domain.Entities.UserAggregate.Enums;

namespace Application.Users.Commands.LoginWithPassword;

public class LoginWithPasswordHandler : IRequestHandler<LoginWithPasswordCommand, Result<UserTokenDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ITokenFactoryService _tokenFactoryService;

    public LoginWithPasswordHandler(IApplicationDbContext dbContext, ITokenFactoryService tokenFactoryService)
    {
        _dbContext = dbContext;
        _tokenFactoryService = tokenFactoryService;
    }

    public async Task<Result<UserTokenDto>> Handle(LoginWithPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.UsersQuery
            .FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (user is null)
            return UserErrors.PhoneNumberOrPasswordIsNotCorrect;

        var isPasswordValid = PasswordHash.VerifyHashedPassword(user.PasswordHash!, request.Password!);
        if (isPasswordValid == false)
            return UserErrors.PhoneNumberOrPasswordIsNotCorrect;

        if (user.Role != UserRole.SuperAdmin)
        {
            return UserErrors.UserIsNotAdmin;
        }

        var accessToken = _tokenFactoryService.CreateJwt(user);

        return new UserTokenDto(accessToken);
    }
}