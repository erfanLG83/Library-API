using Application.Books.Queries.SearchBooks;
using Application.Common.Interfaces;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.UserArea;

[ApiVersion("1")]
[Authorize]
[Route("api/v{version:apiVersion}/books")]
public class BooksController : ApiController
{
    public BooksController(IMediator mediator, ICurrentUserService currentUserService) : base(mediator, currentUserService)
    {
    }

    [HttpGet]
    public async Task<ActionResult<SearchBooksResponse>> Search([FromQuery] SearchBooksQuery query)
    {
        var response = await _mediator.Send(query, CancellationToken);

        return Ok(response);
    }
}
