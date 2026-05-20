using AutoMapper;
using Core.Application.DTOs;
using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.Projects.Queries;

public record GetAllProjectsQuery() : IRequest<RequestResult<List<ProjectDto>>>;

public class GetAllProjectsQueryHandler(IRepository<Project> repository, IMapper mapper)
    : IRequestHandler<GetAllProjectsQuery, RequestResult<List<ProjectDto>>>
{
    public async Task<RequestResult<List<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await repository.GetAllAsync(cancellationToken);
        var dto = mapper.Map<List<ProjectDto>>(projects);
        
        return RequestResult<List<ProjectDto>>.Success(dto);
    }
}
