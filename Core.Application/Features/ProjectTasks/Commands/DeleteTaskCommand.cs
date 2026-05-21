using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.ProjectTasks.Commands;

public record DeleteTaskCommand(Guid Id) : IRequest<RequestResult<bool>>;

public class DeleteTaskCommandHandler(IRepository<ProjectTask> taskRepository, ICacheService cacheService)
    : IRequestHandler<DeleteTaskCommand, RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.Id, cancellationToken);
        if (task is null)
            return RequestResult<bool>.Failure("Task not found.");

        taskRepository.Delete(task);
        await taskRepository.SaveChangesAsync(cancellationToken);

        await cacheService.InvalidateGroupAsync($"Tasks_Proj_{task.ProjectId}", cancellationToken);

        return RequestResult<bool>.Success(true, "Task deleted successfully.");
    }
}
