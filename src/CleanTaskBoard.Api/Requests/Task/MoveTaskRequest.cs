namespace CleanTaskBoard.Api.Requests.Task;

public record MoveTaskRequest(Guid TargetColumnId, int TargetPosition);
