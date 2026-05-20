using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.Projects.Commands;

public record DeleteProjectCommand(Guid Id) : IRequest<RequestResult<bool>>;

public class DeleteProjectCommandHandler(IRepository<Project> repository)
    : IRequestHandler<DeleteProjectCommand, RequestResult<bool>>
{
    public async Task<RequestResult<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (project is null)
            return RequestResult<bool>.Failure("Project not found.");

        repository.Delete(project);
        await repository.SaveChangesAsync(cancellationToken);

        return RequestResult<bool>.Success(true, "Project deleted successfully.");
    }
}
