using Core.Domain.Enums;

namespace Core.Application.Features.ProjectTasks.DTOs;

public class ProjectTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int Status { get; set; }
    public DateTime? DueDate { get; set; }
    public int Priority { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
}
