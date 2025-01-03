using Api.Authorization;
using Application.Categories.Commands.Create;
using Application.Categories.Commands.Delete;
using Application.Categories.Commands.Update;
using Application.Categories.Queries.GetAll;
using Application.Common.Interfaces;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AdminArea;

[ApiVersion("1")]
[Authorize(Policy = AppAuthorizationPolicies.SuperAdminPolicy)]
[Route("api/v{version:apiVersion}/admin/categories")]
public class AdminCategoriesController : ApiController
{
    public AdminCategoriesController(IMediator mediator, ICurrentUserService currentUserService) : base(mediator, currentUserService)
    {
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        await _mediator.Send(command, CancellationToken);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateCategoryCommand command)
    {
        var result = await _mediator.Send(command, CancellationToken);

        return result.ToHttpResponse();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] DeleteCategoryCommand command)
    {
        var result = await _mediator.Send(command, CancellationToken);

        return result.ToHttpResponse();
    }

    [HttpGet]
    public async Task<ActionResult<GetAllCategoriesResponse>> GetAll([FromQuery] GetAllCategoriesQuery query)
    {
        var response = await _mediator.Send(query, CancellationToken);

        return Ok(response);
    }
}
