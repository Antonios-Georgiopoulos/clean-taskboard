namespace CleanTaskBoard.Application.Models.Boards;

public record BoardMemberDto(
    Guid UserId,
    string Email,
    string Role // "Owner", "Member", "Viewer"
);
