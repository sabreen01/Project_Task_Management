using Core.Application.Features.Projects.Commands;
using Core.Application.Features.Projects.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

public class ProjectsController(IMediator mediator) : BaseController
{
    [HttpPost]
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

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateProjectCommand command)
    {
        return HandleResult(await mediator.Send(command));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        return HandleResult(await mediator.Send(new DeleteProjectCommand(id)));
    }

   
}
