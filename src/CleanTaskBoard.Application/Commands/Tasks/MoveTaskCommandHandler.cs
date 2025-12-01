using CleanTaskBoard.Application.Common.Exceptions;
using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class MoveTaskCommandHandler : IRequestHandler<MoveTaskCommand, bool>
{
    private readonly ITaskItemRepository _taskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public MoveTaskCommandHandler(
        ITaskItemRepository taskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _taskRepo = taskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<bool> Handle(MoveTaskCommand request, CancellationToken cancellationToken)
    {
        await _boardAccessService.EnsureCanEditTask(
            request.TaskId,
            request.CurrentUserId,
            cancellationToken
        );

        await _boardAccessService.EnsureCanEditTasksForColumn(
            request.TargetColumnId,
            request.CurrentUserId,
            cancellationToken
        );

        var task = await _taskRepo.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null)
            throw new NotFoundException("TaskItem", request.TaskId);

        var sourceColumnId = task.ColumnId;

        if (sourceColumnId == request.TargetColumnId)
        {
            var tasksInColumn = await _taskRepo.GetByColumnIdAsync(
                sourceColumnId,
                cancellationToken
            );

            tasksInColumn = tasksInColumn.OrderBy(t => t.Order).ToList();

            var moving = tasksInColumn.FirstOrDefault(t => t.Id == task.Id);
            if (moving is null)
                return false;

            tasksInColumn.Remove(moving);

            var pos = request.TargetPosition;
            if (pos < 0)
                pos = 0;
            if (pos > tasksInColumn.Count)
                pos = tasksInColumn.Count;

            tasksInColumn.Insert(pos, moving);

            for (int i = 0; i < tasksInColumn.Count; i++)
            {
                tasksInColumn[i].Order = i;
                await _taskRepo.UpdateAsync(tasksInColumn[i], cancellationToken);
            }

            return true;
        }

        var sourceTasks = await _taskRepo.GetByColumnIdAsync(sourceColumnId, cancellationToken);

        sourceTasks = sourceTasks.OrderBy(t => t.Order).ToList();

        var sourceMoving = sourceTasks.FirstOrDefault(t => t.Id == task.Id);
        if (sourceMoving is null)
            return false;

        sourceTasks.Remove(sourceMoving);

        for (int i = 0; i < sourceTasks.Count; i++)
        {
            sourceTasks[i].Order = i;
            await _taskRepo.UpdateAsync(sourceTasks[i], cancellationToken);
        }

        var targetTasks = await _taskRepo.GetByColumnIdAsync(
            request.TargetColumnId,
            cancellationToken
        );

        targetTasks = targetTasks.OrderBy(t => t.Order).ToList();

        var targetPos = request.TargetPosition;
        if (targetPos < 0)
            targetPos = 0;
        if (targetPos > targetTasks.Count)
            targetPos = targetTasks.Count;

        task.ColumnId = request.TargetColumnId;

        targetTasks.Insert(targetPos, task);

        for (int i = 0; i < targetTasks.Count; i++)
        {
            targetTasks[i].ColumnId = request.TargetColumnId;
            targetTasks[i].Order = i;
            await _taskRepo.UpdateAsync(targetTasks[i], cancellationToken);
        }

        return true;
    }
}
