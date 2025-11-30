namespace CleanTaskBoard.Application.Interfaces.Services;

public interface IBoardAccessService
{
    // Board-level
    Task EnsureCanReadBoard(Guid boardId, Guid userId, CancellationToken cancellationToken);
    Task EnsureCanEditBoard(Guid boardId, Guid userId, CancellationToken cancellationToken);
    Task EnsureCanManageMembership(Guid boardId, Guid userId, CancellationToken cancellationToken);

    // Column-level (by ColumnId)
    Task EnsureCanReadColumn(Guid columnId, Guid userId, CancellationToken cancellationToken);
    Task EnsureCanEditColumn(Guid columnId, Guid userId, CancellationToken cancellationToken);

    // Column-level (by BoardId)
    Task EnsureCanEditColumnsForBoard(
        Guid boardId,
        Guid userId,
        CancellationToken cancellationToken
    );

    // Task-level
    Task EnsureCanReadTask(Guid taskId, Guid userId, CancellationToken cancellationToken);
    Task EnsureCanEditTask(Guid taskId, Guid userId, CancellationToken cancellationToken);

    // Task-level (by ColumnId)
    Task EnsureCanEditTasksForColumn(
        Guid columnId,
        Guid userId,
        CancellationToken cancellationToken
    );

    // Subtask-level
    Task EnsureCanReadSubtask(Guid subtaskId, Guid userId, CancellationToken cancellationToken);
    Task EnsureCanEditSubtask(Guid subtaskId, Guid userId, CancellationToken cancellationToken);

    // Subtask-level (by TaskId)
    Task EnsureCanEditSubtasksForTask(
        Guid taskId,
        Guid userId,
        CancellationToken cancellationToken
    );
}
