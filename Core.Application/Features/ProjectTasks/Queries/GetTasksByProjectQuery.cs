using AutoMapper;
using Core.Application.DTOs;
using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.ProjectTasks.Queries;

public record GetTasksByProjectQuery(Guid ProjectId, int PageNumber = 1, int PageSize = 10) 
    : IRequest<RequestResult<PaginatedResult<ProjectTaskDto>>>;

public class GetTasksByProjectQueryHandler(IRepository<ProjectTask> taskRepository, IMapper mapper)
    : IRequestHandler<GetTasksByProjectQuery, RequestResult<PaginatedResult<ProjectTaskDto>>>
{
    public async Task<RequestResult<PaginatedResult<ProjectTaskDto>>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        var (tasks, totalCount) = await taskRepository.GetPaginatedAsync(
            t => t.ProjectId == request.ProjectId,
            request.PageNumber, request.PageSize, cancellationToken);
        
        var dto = mapper.Map<List<ProjectTaskDto>>(tasks);
        var paginatedResult = new PaginatedResult<ProjectTaskDto>(dto, totalCount, request.PageNumber, request.PageSize);
        
        return RequestResult<PaginatedResult<ProjectTaskDto>>.Success(paginatedResult);
    }
}
