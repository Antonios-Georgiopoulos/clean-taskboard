namespace CleanTaskBoard.Api.Requests.Task;

public record UpdateTaskRequest(string Title, string? Description, DateTime? DueDate, int Priority);
