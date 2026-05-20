using Core.Application.Features.ProjectTasks.Commands;
using Core.Application.Features.ProjectTasks.Queries;
using Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

public class TasksController(IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateTaskCommand command)
    {
        return HandleResult(await mediator.Send(command));
    }
    
    [HttpGet("project/{projectId:guid}")]
    public async Task<ActionResult> GetByProject(Guid projectId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        return HandleResult(await mediator.Send(new GetTasksByProjectQuery(projectId, pageNumber, pageSize)));
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult> UpdateStatus(Guid id, [FromBody] TaskStatusOptions status)
    {
        return HandleResult(await mediator.Send(new UpdateTaskStatusCommand(id, status)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        return HandleResult(await mediator.Send(new DeleteTaskCommand(id)));
    }
    
}
