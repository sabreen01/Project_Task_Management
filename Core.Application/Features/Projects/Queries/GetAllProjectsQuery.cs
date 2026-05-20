using AutoMapper;
using Core.Application.DTOs;
using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.Projects.Queries;

public record GetAllProjectsQuery(int PageNumber = 1, int PageSize = 10) 
    : IRequest<RequestResult<PaginatedResult<ProjectDto>>>;

public class GetAllProjectsQueryHandler(IRepository<Project> repository, IMapper mapper)
    : IRequestHandler<GetAllProjectsQuery, RequestResult<PaginatedResult<ProjectDto>>>
{
    public async Task<RequestResult<PaginatedResult<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var (projects, totalCount) = await repository.GetPaginatedAsync(
            request.PageNumber, request.PageSize, cancellationToken);
        
        var dto = mapper.Map<List<ProjectDto>>(projects);
        var paginatedResult = new PaginatedResult<ProjectDto>(dto, totalCount, request.PageNumber, request.PageSize);
        
        return RequestResult<PaginatedResult<ProjectDto>>.Success(paginatedResult);
    }
}
