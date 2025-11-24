namespace CleanTaskBoard.Domain.Entities;

public class Subtask
{
    public Guid Id { get; set; }
    public Guid TaskItemId { get; set; }

    public string Title { get; set; } = default!;
    public bool IsCompleted { get; set; }

    public int Order { get; set; }
}
