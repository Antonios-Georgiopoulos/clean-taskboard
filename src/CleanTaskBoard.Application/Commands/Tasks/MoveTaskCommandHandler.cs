using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class MoveTaskCommandHandler : IRequestHandler<MoveTaskCommand, bool>
{
    private readonly ITaskItemRepository _taskRepo;

    public MoveTaskCommandHandler(ITaskItemRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<bool> Handle(MoveTaskCommand request, CancellationToken cancellationToken)
    {
        // 1) Load the task
        var task = await _taskRepo.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null)
            return false;

        // 2) Load all tasks from the target column
        var tasksInColumn = await _taskRepo.GetByColumnIdAsync(
            request.TargetColumnId,
            cancellationToken
        );

        // 3) Remove the task if it already exists in this column (moving inside same column)
        tasksInColumn.RemoveAll(t => t.Id == task.Id);

        // 4) Insert task at the correct position
        if (request.TargetPosition < 0)
            request = request with { TargetPosition = 0 };

        if (request.TargetPosition > tasksInColumn.Count)
            request = request with { TargetPosition = tasksInColumn.Count };

        tasksInColumn.Insert(request.TargetPosition, task);

        // 5) Reorder (stable ordering: 0,1,2...)
        for (int i = 0; i < tasksInColumn.Count; i++)
        {
            tasksInColumn[i].ColumnId = request.TargetColumnId;
            tasksInColumn[i].Order = i;
            await _taskRepo.UpdateAsync(tasksInColumn[i], cancellationToken);
        }

        return true;
    }
}
