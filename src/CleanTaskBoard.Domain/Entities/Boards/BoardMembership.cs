using CleanTaskBoard.Domain.Entities.Users;

namespace CleanTaskBoard.Domain.Entities.Boards;

public class BoardMembership
{
    public Guid Id { get; set; }

    public Guid BoardId { get; set; }
    public Guid UserId { get; set; }

    public BoardRole Role { get; set; }
    public Board? Board { get; set; }
    public User? User { get; set; }
}
