using Core.Application.Features.Projects.Commands;
using Core.Application.Features.Projects.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

[Authorize]
public class ProjectsController(IMediator mediator) : BaseController
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Create([FromBody] CreateProjectCommand command)
    {
        return HandleResult(await mediator.Send(command));
    }
    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        return HandleResult(await mediator.Send(new GetAllProjectsQuery(pageNumber, pageSize)));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        return HandleResult(await mediator.Send(new GetProjectByIdQuery(id)));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateProjectCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id mismatch");

        return HandleResult(await mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        return HandleResult(await mediator.Send(new DeleteProjectCommand(id)));
    }

   
}
