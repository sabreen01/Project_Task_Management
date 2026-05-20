using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.Projects.Commands;

public record UpdateProjectCommand(Guid Id, string Name, string? Description) : IRequest<RequestResult<Guid>>;

public class UpdateProjectCommandHandler(IRepository<Project> repository)
    : IRequestHandler<UpdateProjectCommand, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (project is null)
            return RequestResult<Guid>.Failure("Project not found.");

        project.Name = request.Name;
        project.Description = request.Description;
        project.UpdatedAt = DateTime.UtcNow;

        repository.Update(project);
        await repository.SaveChangesAsync(cancellationToken);

        return RequestResult<Guid>.Success(project.Id, "Project updated successfully.");
    }
}
