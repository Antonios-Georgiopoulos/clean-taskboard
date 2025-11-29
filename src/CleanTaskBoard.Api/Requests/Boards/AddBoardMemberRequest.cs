namespace CleanTaskBoard.Api.Requests.Boards;

public class AddBoardMemberRequest
{
    public Guid MemberUserId { get; set; }
    public string Role { get; set; } = default!; // "Member" ή "Viewer"
}
