namespace Core.Application.Features.Projects.DTOs;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
