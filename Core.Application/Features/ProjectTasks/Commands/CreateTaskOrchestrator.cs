using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Enums;
using MediatR;

namespace Core.Application.Features.ProjectTasks.Commands;

public record CreateTaskOrchestrator(Guid ProjectId, string Title, string? Description, DateTime? DueDate, TaskPriority Priority) : IRequest<RequestResult<Guid>>;

public class CreateTaskOrchestratorHandler(
    IRepository<ProjectTask> taskRepository,
    IRepository<Project> projectRepository)
    : IRequestHandler<CreateTaskOrchestrator, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(CreateTaskOrchestrator request, CancellationToken cancellationToken)
    {
        // Orchestration step 1: Validate Project exists
        var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
            return RequestResult<Guid>.Failure("The specified project does not exist.");

        // Orchestration step 2: Create the Task
        var task = new ProjectTask
        {
            ProjectId = request.ProjectId,
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Priority = request.Priority,
            Status = TaskStatusOptions.Todo
        };

        await taskRepository.AddAsync(task, cancellationToken);
        await taskRepository.SaveChangesAsync(cancellationToken);

        return RequestResult<Guid>.Success(task.Id, "Task created successfully within the project.");
    }
}
