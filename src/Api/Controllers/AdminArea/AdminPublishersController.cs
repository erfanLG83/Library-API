using Api.Authorization;
using Application.Common.Interfaces;
using Application.Publishers.Queries.GetAll;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminArea;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/admin/publishers")]
[Authorize(Policy = AppAuthorizationPolicies.SuperAdminPolicy)]
public class AdminPublishersController : ApiController
{
    public AdminPublishersController(IMediator mediator, ICurrentUserService currentUserService) : base(mediator, currentUserService)
    {
    }

    [HttpGet]
    public async Task<ActionResult<GetAllPublishersResponse>> GetAll([FromQuery] GetAllPublishersQuery query)
    {
        var response = await _mediator.Send(query, CancellationToken);

        return Ok(response);
    }
}
