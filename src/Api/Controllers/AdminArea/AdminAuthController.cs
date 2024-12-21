using Application.Common.Interfaces;
using Application.Users.Commands.LoginWithPassword;
using Application.Users.Common.DTOs;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminArea;

[ApiVersion("1")]
[Tags("admin-area-auth")]
[Route("api/v{version:apiVersion}/admin/auth")]
public class AdminAuthController : ApiController
{
    public AdminAuthController(IMediator mediator, ICurrentUserService currentUserService) : base(mediator, currentUserService)
    {
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserTokenDto>> Login([FromBody] LoginWithPasswordDto dto)
    {
        var command = new LoginWithPasswordCommand
        {
            Password = dto.Password,
            PhoneNumber = dto.PhoneNumber,
        };

        var result = await _mediator.Send(command, CancellationToken);

        return result.ToHttpResponse();
    }
}
