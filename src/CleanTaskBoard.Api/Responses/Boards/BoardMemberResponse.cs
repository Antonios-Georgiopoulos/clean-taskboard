namespace CleanTaskBoard.Api.Responses.Boards;

public class BoardMemberResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
}
