using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class ProjectTask : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public TaskStatusOptions Status { get; set; } = TaskStatusOptions.Todo;
    public DateTime? DueDate { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}
