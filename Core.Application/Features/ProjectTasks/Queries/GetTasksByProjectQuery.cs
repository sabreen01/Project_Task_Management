using AutoMapper;
using Core.Application.DTOs;
using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.ProjectTasks.Queries;

public record GetTasksByProjectQuery(Guid ProjectId) : IRequest<RequestResult<List<ProjectTaskDto>>>;

public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, RequestResult<List<ProjectTaskDto>>>
{
    private readonly IRepository<ProjectTask> _taskRepository;
    private readonly IMapper _mapper;

    public GetTasksByProjectQueryHandler(IRepository<ProjectTask> taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<RequestResult<List<ProjectTaskDto>>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        var allTasks = await _taskRepository.GetAllAsync(cancellationToken);
        var projectTasks = allTasks.Where(t => t.ProjectId == request.ProjectId).ToList();
        
        var dto = _mapper.Map<List<ProjectTaskDto>>(projectTasks);
        return RequestResult<List<ProjectTaskDto>>.Success(dto);
    }
}
