using AutoMapper;
using Core.Application.Features.ProjectTasks.DTOs;
using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.ProjectTasks.Queries;

public record GetTasksByProjectQuery(Guid ProjectId, int PageNumber = 1, int PageSize = 10) 
    : IRequest<RequestResult<PaginatedResult<ProjectTaskDto>>>;

public class GetTasksByProjectQueryHandler(IRepository<ProjectTask> taskRepository)
    : IRequestHandler<GetTasksByProjectQuery, RequestResult<PaginatedResult<ProjectTaskDto>>>
{
    public async Task<RequestResult<PaginatedResult<ProjectTaskDto>>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        var query = taskRepository.GetAll(t => t.ProjectId == request.ProjectId);
        var totalCount = await query.CountAsync(cancellationToken);
        
        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new ProjectTaskDto
            {
                Id = t.Id,
                ProjectId = t.ProjectId,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = (int)t.Priority,
                Status = (int)t.Status
            })
            .ToListAsync(cancellationToken);

        var paginatedResult = new PaginatedResult<ProjectTaskDto>(tasks, totalCount, request.PageNumber, request.PageSize);
        return RequestResult<PaginatedResult<ProjectTaskDto>>.Success(paginatedResult);
    }
}
