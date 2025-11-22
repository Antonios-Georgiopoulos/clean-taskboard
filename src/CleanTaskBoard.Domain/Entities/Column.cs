namespace CleanTaskBoard.Domain.Entities;

public class Column
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = default!;
    public int Order { get; set; }
}
