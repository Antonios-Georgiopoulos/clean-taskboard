using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class ReorderTaskCommandHandler : IRequestHandler<ReorderTaskCommand, bool>
{
    private readonly ITaskItemRepository _taskRepo;

    public ReorderTaskCommandHandler(ITaskItemRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<bool> Handle(ReorderTaskCommand request, CancellationToken cancellationToken)
    {
        // Load the task
        var task = await _taskRepo.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null)
            return false;

        var columnId = task.ColumnId;

        // Load all tasks from the column
        var tasks = await _taskRepo.GetByColumnIdAsync(columnId, cancellationToken);

        // Remove current task
        tasks.RemoveAll(t => t.Id == task.Id);

        // Normalize target position
        var pos = request.TargetPosition;
        if (pos < 0)
            pos = 0;
        if (pos > tasks.Count)
            pos = tasks.Count;

        // Insert task at new position
        tasks.Insert(pos, task);

        // Reorder (0,1,2…)
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].Order = i;
            await _taskRepo.UpdateAsync(tasks[i], cancellationToken);
        }

        return true;
    }
}
