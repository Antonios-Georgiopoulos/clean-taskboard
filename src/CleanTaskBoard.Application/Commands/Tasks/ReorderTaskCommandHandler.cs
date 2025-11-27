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
        var task = await _taskRepo.GetByIdAsync(
            request.TaskId,
            request.OwnerUserId,
            cancellationToken
        );
        if (task is null)
            return false;

        var columnId = task.ColumnId;

        var tasks = await _taskRepo.GetByColumnIdAsync(
            columnId,
            request.OwnerUserId,
            cancellationToken
        );

        tasks.RemoveAll(t => t.Id == task.Id);

        var pos = request.TargetPosition;
        if (pos < 0)
            pos = 0;
        if (pos > tasks.Count)
            pos = tasks.Count;

        tasks.Insert(pos, task);

        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].Order = i;
            await _taskRepo.UpdateAsync(tasks[i], cancellationToken);
        }

        return true;
    }
}
