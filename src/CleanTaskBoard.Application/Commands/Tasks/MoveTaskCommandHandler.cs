using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class MoveTaskCommandHandler : IRequestHandler<MoveTaskCommand, bool>
{
    private readonly ITaskItemRepository _taskRepo;
    private readonly IColumnRepository _columnRepo;

    public MoveTaskCommandHandler(ITaskItemRepository taskRepo, IColumnRepository columnRepo)
    {
        _taskRepo = taskRepo;
        _columnRepo = columnRepo;
    }

    public async Task<bool> Handle(MoveTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepo.GetByIdAsync(
            request.TaskId,
            request.OwnerUserId,
            cancellationToken
        );
        if (task is null)
            return false;

        var targetColumn = await _columnRepo.GetByIdAsync(
            request.TargetColumnId,
            request.OwnerUserId,
            cancellationToken
        );
        if (targetColumn is null)
            return false;

        var tasksInColumn = await _taskRepo.GetByColumnIdAsync(
            request.TargetColumnId,
            request.OwnerUserId,
            cancellationToken
        );

        tasksInColumn.RemoveAll(t => t.Id == task.Id);

        var pos = request.TargetPosition;
        if (pos < 0)
            pos = 0;
        if (pos > tasksInColumn.Count)
            pos = tasksInColumn.Count;

        tasksInColumn.Insert(pos, task);

        for (int i = 0; i < tasksInColumn.Count; i++)
        {
            tasksInColumn[i].ColumnId = request.TargetColumnId;
            tasksInColumn[i].Order = i;
            await _taskRepo.UpdateAsync(tasksInColumn[i], cancellationToken);
        }

        return true;
    }
}
