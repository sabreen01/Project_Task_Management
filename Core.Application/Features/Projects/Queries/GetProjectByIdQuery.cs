using AutoMapper;
using Core.Application.DTOs;
using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.Projects.Queries;

public record GetProjectByIdQuery(Guid Id) : IRequest<RequestResult<ProjectDto>>;

public class GetProjectByIdQueryHandler(IRepository<Project> repository, IMapper mapper)
    : IRequestHandler<GetProjectByIdQuery, RequestResult<ProjectDto>>
{
    public async Task<RequestResult<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (project is null)
            return RequestResult<ProjectDto>.Failure("Project not found.");

        var dto = mapper.Map<ProjectDto>(project);
        return RequestResult<ProjectDto>.Success(dto);
    }
}
