using Core.Application.Features.ProjectTasks.Commands;
using Core.Application.Features.ProjectTasks.Queries;
using Core.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers;

public class TasksController(IMediator mediator) : BaseController
{
    [HttpPost("orchestrator")]
    public async Task<ActionResult> Create([FromBody] CreateTaskOrchestrator command)
    {
        return HandleResult(await mediator.Send(command));
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

    [HttpGet("project/{projectId:guid}")]
    public async Task<ActionResult> GetByProject(Guid projectId)
    {
        return HandleResult(await mediator.Send(new GetTasksByProjectQuery(projectId)));
    }
}
