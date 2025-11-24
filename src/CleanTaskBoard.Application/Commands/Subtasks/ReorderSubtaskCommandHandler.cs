using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public class ReorderSubtaskCommandHandler : IRequestHandler<ReorderSubtaskCommand, bool>
{
    private readonly ISubtaskRepository _subtaskRepo;

    public ReorderSubtaskCommandHandler(ISubtaskRepository subtaskRepo)
    {
        _subtaskRepo = subtaskRepo;
    }

    public async Task<bool> Handle(
        ReorderSubtaskCommand request,
        CancellationToken cancellationToken
    )
    {
        var subtask = await _subtaskRepo.GetByIdAsync(request.SubtaskId, cancellationToken);
        if (subtask is null)
            return false;

        var taskId = subtask.TaskItemId;

        var subtasks = await _subtaskRepo.GetByTaskIdAsync(taskId, cancellationToken);
        subtasks.RemoveAll(s => s.Id == subtask.Id);

        var pos = request.TargetPosition;
        if (pos < 0)
            pos = 0;
        if (pos > subtasks.Count)
            pos = subtasks.Count;

        subtasks.Insert(pos, subtask);

        for (int i = 0; i < subtasks.Count; i++)
        {
            subtasks[i].Order = i;
            await _subtaskRepo.UpdateAsync(subtasks[i], cancellationToken);
        }

        return true;
    }
}
