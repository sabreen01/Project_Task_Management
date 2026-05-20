using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Enums;
using MediatR;

namespace Core.Application.Features.ProjectTasks.Commands;

public record CreateTaskCommand(Guid ProjectId, string Title, string? Description, DateTime? DueDate, TaskPriority Priority) : IRequest<RequestResult<Guid>>;

public class CreateTaskCommandHandler(
    IRepository<ProjectTask> taskRepository,
    IRepository<Project> projectRepository)
    : IRequestHandler<CreateTaskCommand, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        
        var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null)
            return RequestResult<Guid>.Failure("The specified project does not exist.");

       
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
