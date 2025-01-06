using Application.Categories.Queries.GetAll;
using Application.Common.Interfaces;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.UserArea;

[ApiVersion("1")]
[Authorize]
[Route("api/v{version:apiVersion}/categories")]
public class CategoriesController : ApiController
{
    public CategoriesController(IMediator mediator, ICurrentUserService currentUserService) : base(mediator, currentUserService)
    {
    }

    [HttpGet]
    public async Task<ActionResult<GetAllCategoriesResponse>> GetAll([FromQuery] GetAllCategoriesQuery query)
    {
        var response = await _mediator.Send(query, CancellationToken);

        return Ok(response);
    }
}
