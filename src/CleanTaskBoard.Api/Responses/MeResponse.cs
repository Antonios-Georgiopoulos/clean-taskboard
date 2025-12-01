namespace CleanTaskBoard.Api.Responses;

public sealed record MeResponse(Guid Id, string Email, DateTime CreatedAtUtc);
