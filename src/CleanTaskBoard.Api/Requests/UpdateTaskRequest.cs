namespace CleanTaskBoard.Api.Requests;

public record UpdateTaskRequest(string Title, string? Description, DateTime? DueDate, int Priority);
