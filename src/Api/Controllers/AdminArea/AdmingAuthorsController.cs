using Application.Authors.Commands.Create;
using Application.Authors.Commands.Update;
using Application.Authors.Queries.GetAll;
using Application.Common.Interfaces;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminArea;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/admin/authors")]
public class AdmingAuthorsController : ApiController
{
    public AdmingAuthorsController(IMediator mediator, ICurrentUserService currentUserService) : base(mediator, currentUserService)
    {
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateAuthorCommand command)
    {
        await _mediator.Send(command, CancellationToken);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateAuthorCommand command)
    {
        var result = await _mediator.Send(command, CancellationToken);

        return result.ToHttpResponse();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] DeleteAuthorCommand command)
    {
        var result = await _mediator.Send(command, CancellationToken);

        return result.ToHttpResponse();
    }

    [HttpGet]
    public async Task<ActionResult<GetAllAuthorsResponse>> GetAll()
    {
        var query = new GetAllAuthorsQuery();
        var response = await _mediator.Send(query, CancellationToken);

        return Ok(response);
    }
}
