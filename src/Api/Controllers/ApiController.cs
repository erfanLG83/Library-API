using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[Controller]")]
public abstract class ApiController : ControllerBase
{
    protected readonly IMediator _mediator;
    protected readonly ICurrentUserService _currentUserService;

    protected ApiController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;
}