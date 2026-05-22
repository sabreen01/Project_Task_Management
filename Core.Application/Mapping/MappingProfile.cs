using AutoMapper;
using Core.Application.Features.Projects.DTOs;
using Core.Application.Features.ProjectTasks.DTOs;
using Core.Domain.Entities;

namespace Core.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectTask, ProjectTaskDto>();
    }
}
