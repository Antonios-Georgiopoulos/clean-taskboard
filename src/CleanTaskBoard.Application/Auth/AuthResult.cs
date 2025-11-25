namespace CleanTaskBoard.Application.Auth;

public record AuthResult(Guid UserId, string Username, string Email, string Token);
