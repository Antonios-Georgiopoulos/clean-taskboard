namespace CleanTaskBoard.Domain.Entities;

public class Board
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
