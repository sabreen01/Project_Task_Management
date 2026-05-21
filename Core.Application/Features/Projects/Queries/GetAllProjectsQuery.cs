using AutoMapper;
using Core.Application.Features.Projects.DTOs;
using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Projects.Queries;

public record GetAllProjectsQuery(int PageNumber = 1, int PageSize = 10) 
    : IRequest<RequestResult<PaginatedResult<ProjectDto>>>;

public class GetAllProjectsQueryHandler(IRepository<Project> repository)
    : IRequestHandler<GetAllProjectsQuery, RequestResult<PaginatedResult<ProjectDto>>>
{
    public async Task<RequestResult<PaginatedResult<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var query = repository.GetAll();
        var totalCount = await query.CountAsync(cancellationToken);
        
        var projects = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            })
            .ToListAsync(cancellationToken);

        var paginatedResult = new PaginatedResult<ProjectDto>(projects, totalCount, request.PageNumber, request.PageSize);
        
        return RequestResult<PaginatedResult<ProjectDto>>.Success(paginatedResult);
    }
}
