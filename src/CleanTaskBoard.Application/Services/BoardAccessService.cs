using CleanTaskBoard.Application.Common.Exceptions;
using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Entities.Boards;

namespace CleanTaskBoard.Application.Services;

public class BoardAccessService : IBoardAccessService
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardMembershipRepository _membershipRepository;
    private readonly IColumnRepository _columnRepository;
    private readonly ITaskItemRepository _taskRepository;
    private readonly ISubtaskRepository _subtaskRepository;

    public BoardAccessService(
        IBoardRepository boardRepository,
        IBoardMembershipRepository membershipRepository,
        IColumnRepository columnRepository,
        ITaskItemRepository taskRepository,
        ISubtaskRepository subtaskRepository
    )
    {
        _boardRepository = boardRepository;
        _membershipRepository = membershipRepository;
        _columnRepository = columnRepository;
        _taskRepository = taskRepository;
        _subtaskRepository = subtaskRepository;
    }

    // ---------------- Board-level ----------------

    public async Task EnsureCanReadBoard(
        Guid boardId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var (_, role) = await GetBoardAndRole(boardId, userId, cancellationToken);

        if (role is BoardRole.Owner or BoardRole.Member or BoardRole.Viewer)
            return;

        throw new ForbiddenAccessException();
    }

    public async Task EnsureCanEditBoard(
        Guid boardId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var (board, role) = await GetBoardAndRole(boardId, userId, cancellationToken);

        if (board.OwnerUserId == userId && role == BoardRole.Owner)
            return;

        throw new ForbiddenAccessException("Only board owner can modify board.");
    }

    public async Task EnsureCanManageMembership(
        Guid boardId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var (board, _) = await GetBoardAndRole(boardId, userId, cancellationToken);

        if (board.OwnerUserId != userId)
            throw new ForbiddenAccessException("Only board owner can manage membership.");
    }

    // ---------------- Column-level ----------------

    public async Task EnsureCanReadColumn(
        Guid columnId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var (_, role) = await GetBoardAndRoleByColumn(columnId, userId, cancellationToken);

        if (role is BoardRole.Owner or BoardRole.Member or BoardRole.Viewer)
            return;

        throw new ForbiddenAccessException();
    }

    public async Task EnsureCanEditColumn(
        Guid columnId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var (board, role) = await GetBoardAndRoleByColumn(columnId, userId, cancellationToken);

        if (board.OwnerUserId == userId && role == BoardRole.Owner)
            return;

        throw new ForbiddenAccessException("Only board owner can modify columns.");
    }

    public async Task EnsureCanEditColumnsForBoard(
        Guid boardId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        // Owner-only, ίδια λογική με EnsureCanEditBoard
        await EnsureCanEditBoard(boardId, userId, cancellationToken);
    }

    // ---------------- Task-level ----------------

    public async Task EnsureCanReadTask(
        Guid taskId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var (_, role) = await GetBoardAndRoleByTask(taskId, userId, cancellationToken);

        if (role is BoardRole.Owner or BoardRole.Member or BoardRole.Viewer)
            return;

        throw new ForbiddenAccessException();
    }

    public async Task EnsureCanEditTask(
        Guid taskId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var (_, role) = await GetBoardAndRoleByTask(taskId, userId, cancellationToken);

        if (role is BoardRole.Owner or BoardRole.Member)
            return;

        throw new ForbiddenAccessException("You cannot modify tasks on this board.");
    }

    public async Task EnsureCanEditTasksForColumn(
        Guid columnId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var (_, role) = await GetBoardAndRoleByColumn(columnId, userId, cancellationToken);

        if (role is BoardRole.Owner or BoardRole.Member)
            return;

        throw new ForbiddenAccessException("You cannot modify tasks on this board.");
    }

    // ---------------- Subtask-level ----------------

    public async Task EnsureCanReadSubtask(
        Guid subtaskId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var (_, role) = await GetBoardAndRoleBySubtask(subtaskId, userId, cancellationToken);

        if (role is BoardRole.Owner or BoardRole.Member or BoardRole.Viewer)
            return;

        throw new ForbiddenAccessException();
    }

    public async Task EnsureCanEditSubtask(
        Guid subtaskId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var (_, role) = await GetBoardAndRoleBySubtask(subtaskId, userId, cancellationToken);

        if (role is BoardRole.Owner or BoardRole.Member)
            return;

        throw new ForbiddenAccessException("You cannot modify subtasks on this board.");
    }

    public async Task EnsureCanEditSubtasksForTask(
        Guid taskId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var (_, role) = await GetBoardAndRoleByTask(taskId, userId, cancellationToken);

        if (role is BoardRole.Owner or BoardRole.Member)
            return;

        throw new ForbiddenAccessException("You cannot modify subtasks on this board.");
    }

    // ---------------- Private helpers ----------------

    private async Task<(Board board, BoardRole role)> GetBoardAndRole(
        Guid boardId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var board = await _boardRepository.GetByIdAsync(boardId, cancellationToken);
        if (board is null)
            throw new NotFoundException("Board", boardId);

        if (board.OwnerUserId == userId)
            return (board, BoardRole.Owner);

        var membership = await _membershipRepository.GetAsync(boardId, userId, cancellationToken);

        if (membership is null)
            throw new NotFoundException("Board", boardId);

        return (board, membership.Role);
    }

    private async Task<(Board board, BoardRole role)> GetBoardAndRoleByColumn(
        Guid columnId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var column = await _columnRepository.GetByIdAsync(columnId, cancellationToken);
        if (column is null)
            throw new NotFoundException("Column", columnId);

        return await GetBoardAndRole(column.BoardId, userId, cancellationToken);
    }

    private async Task<(Board board, BoardRole role)> GetBoardAndRoleByTask(
        Guid taskId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);
        if (task is null)
            throw new NotFoundException("TaskItem", taskId);

        var column = await _columnRepository.GetByIdAsync(task.ColumnId, cancellationToken);
        if (column is null)
            throw new NotFoundException("Column", task.ColumnId);

        return await GetBoardAndRole(column.BoardId, userId, cancellationToken);
    }

    private async Task<(Board board, BoardRole role)> GetBoardAndRoleBySubtask(
        Guid subtaskId,
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var subtask = await _subtaskRepository.GetByIdAsync(subtaskId, cancellationToken);
        if (subtask is null)
            throw new NotFoundException("Subtask", subtaskId);

        var task = await _taskRepository.GetByIdAsync(subtask.TaskItemId, cancellationToken);
        if (task is null)
            throw new NotFoundException("TaskItem", subtask.TaskItemId);

        var column = await _columnRepository.GetByIdAsync(task.ColumnId, cancellationToken);
        if (column is null)
            throw new NotFoundException("Column", task.ColumnId);

        return await GetBoardAndRole(column.BoardId, userId, cancellationToken);
    }
}
