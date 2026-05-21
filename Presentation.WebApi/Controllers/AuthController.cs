using Core.Application.Features.Authentication.Commands;
using Core.Application.Features.Authentication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

public class AuthController(IMediator mediator) : BaseController
{
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterCommand command)
    {
        return HandleResult(await mediator.Send(command));
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginQuery query)
    {
        return HandleResult(await mediator.Send(query));
    }
}
