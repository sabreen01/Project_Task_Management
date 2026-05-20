using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.Projects.Commands;

public record CreateProjectCommand(string Name, string? Description) : IRequest<RequestResult<Guid>>;

public class CreateProjectCommandHandler(IRepository<Project> repository)
    : IRequestHandler<CreateProjectCommand, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Name = request.Name,
            Description = request.Description
        };

        await repository.AddAsync(project, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return RequestResult<Guid>.Success(project.Id, "Project created successfully.");
    }
}
