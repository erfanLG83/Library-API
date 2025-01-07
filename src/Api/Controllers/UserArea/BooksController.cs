using Application.Books.Commands.BorrowBook;
using Application.Books.Queries.GetBookDetails;
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

    [HttpGet("{id}")]
    public async Task<ActionResult<GetBookDetailsResponse>> GetDetails([FromRoute] string id)
    {
        var query = new GetBookDetailsQuery(id, _currentUserService.UserId!);
        var response = await _mediator.Send(query, CancellationToken);

        return Ok(response);
    }

    [HttpPost("{id}/borrow")]
    public async Task<ActionResult<GetBookDetailsResponse>> Borrow([FromRoute] string id, [FromBody] BorrowBookDto dto)
    {
        var command = new BorrowBookCommand(_currentUserService.UserId!, id, dto.Branch);
        var result = await _mediator.Send(command, CancellationToken);

        return result.ToHttpResponse();
    }
}
