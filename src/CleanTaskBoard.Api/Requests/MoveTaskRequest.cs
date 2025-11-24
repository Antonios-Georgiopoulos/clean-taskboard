namespace CleanTaskBoard.Api.Requests;

public record MoveTaskRequest(Guid TargetColumnId, int TargetPosition);
