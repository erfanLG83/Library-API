using Application.Authors.Queries.GetAll;
using Application.Common.Interfaces;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.UserArea;

[ApiVersion("1")]
[Authorize]
[Route("api/v{version:apiVersion}/authors")]
public class AuthorsController : ApiController
{
    public AuthorsController(IMediator mediator, ICurrentUserService currentUserService) : base(mediator, currentUserService)
    {
    }

    [HttpGet]
    public async Task<ActionResult<GetAllAuthorsResponse>> GetAll([FromQuery] GetAllAuthorsQuery query)
    {
        var response = await _mediator.Send(query, CancellationToken);

        return Ok(response);
    }
}
