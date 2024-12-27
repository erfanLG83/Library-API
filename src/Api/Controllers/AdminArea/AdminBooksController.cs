using Application.Books.Commands.Create;
using Application.Books.Commands.Delete;
using Application.Books.Commands.Update;
using Application.Books.Queries.GetAll;
using Application.Common.Interfaces;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminArea;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/admin/books")]
public class AdminBooksController : ApiController
{
    public AdminBooksController(IMediator mediator, ICurrentUserService currentUserService) : base(mediator, currentUserService)
    {
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateBookCommand command)
    {
        await _mediator.Send(command, CancellationToken);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateBookCommand command)
    {
        var result = await _mediator.Send(command, CancellationToken);

        return result.ToHttpResponse();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] DeleteBookCommand command)
    {
        var result = await _mediator.Send(command, CancellationToken);

        return result.ToHttpResponse();
    }

    [HttpGet]
    public async Task<ActionResult<GetAllBooksResponse>> GetAll([FromQuery] GetAllBooksQuery query)
    {
        var response = await _mediator.Send(query, CancellationToken);

        return Ok(response);
    }
}
