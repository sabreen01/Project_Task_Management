using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Enums;
using MediatR;

namespace Core.Application.Features.ProjectTasks.Commands;

public record UpdateTaskStatusCommand(Guid Id, TaskStatusOptions Status) : IRequest<RequestResult<Guid>>;

public class UpdateTaskStatusCommandHandler(IRepository<ProjectTask> taskRepository)
    : IRequestHandler<UpdateTaskStatusCommand, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.Id, cancellationToken);
        if (task is null)
            return RequestResult<Guid>.Failure("Task not found.");

        task.Status = request.Status;
        task.UpdatedAt = DateTime.UtcNow;

        taskRepository.Update(task);
        await taskRepository.SaveChangesAsync(cancellationToken);

        return RequestResult<Guid>.Success(task.Id, "Task status updated successfully.");
    }
}
