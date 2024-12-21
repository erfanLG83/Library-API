using Application.Common.Interfaces;
using Application.Users.Commands.SendOtpCode;
using Application.Users.Commands.VerifyPhoneNumber;
using Application.Users.Common.DTOs;
using Application.Users.Queries.GetUserInfo;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.UserArea;

[ApiVersion(1)]
public class AuthController : ApiController
{
    public AuthController(IMediator mediator, ICurrentUserService currentUserService) : base(mediator, currentUserService)
    {
    }

    [HttpPost("SendOtpCode")]
    public async Task<ActionResult<SendOtpCodeResponse>> SendOtpCode([FromBody] SendOtpCodeCommand command)
    {
        var result = await _mediator.Send(command, CancellationToken);

        return result.ToHttpResponse();
    }

    [HttpPost("VerifyPhoneNumber")]
    public async Task<ActionResult<UserTokenDto>> VerifyPhoneNumber([FromBody] VerifyPhoneNumberCommand command)
    {
        var result = await _mediator.Send(command, CancellationToken);

        return result.ToHttpResponse();
    }

    [HttpGet("userInfo")]
    [Authorize]
    public async Task<ActionResult<GetUserInfoResponse>> GetUserInfo()
    {
        var query = new GetUserInfoQuery(_currentUserService.UserId!);
        var response = await _mediator.Send(query, CancellationToken);

        return Ok(response);
    }
}