namespace CleanTaskBoard.Api.Requests;

public record CreateTaskRequest(
    string Title,
    string? Description,
    DateTime? DueDate,
    int Priority // 1=Low, 2=Medium, 3=High
);
