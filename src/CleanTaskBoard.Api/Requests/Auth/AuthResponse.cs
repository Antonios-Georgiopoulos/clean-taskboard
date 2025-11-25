namespace CleanTaskBoard.Api.Requests.Auth;

public record AuthResponse(Guid UserId, string Username, string Email, string Token);
