using CleanTaskBoard.Domain.Enums;

public class TaskItem
{
    public Guid Id { get; set; }
    public Guid ColumnId { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    public bool IsCompleted { get; set; }
    public int Order { get; set; }
}
