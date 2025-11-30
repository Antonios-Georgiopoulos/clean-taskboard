using CleanTaskBoard.Application.Common.Exceptions;
using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public class ReorderSubtaskCommandHandler : IRequestHandler<ReorderSubtaskCommand, bool>
{
    private readonly ISubtaskRepository _subtaskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public ReorderSubtaskCommandHandler(
        ISubtaskRepository subtaskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _subtaskRepo = subtaskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<bool> Handle(
        ReorderSubtaskCommand request,
        CancellationToken cancellationToken
    )
    {
        await _boardAccessService.EnsureCanEditSubtask(
            request.SubtaskId,
            request.CurrentUserId,
            cancellationToken
        );

        var subtask = await _subtaskRepo.GetByIdAsync(request.SubtaskId, cancellationToken);
        if (subtask is null)
            throw new NotFoundException("Subtask", request.SubtaskId);

        var subtasks = await _subtaskRepo.GetByTaskIdAsync(subtask.TaskItemId, cancellationToken);

        subtasks = subtasks.OrderBy(s => s.Order).ToList();

        var moving = subtasks.FirstOrDefault(s => s.Id == request.SubtaskId);
        if (moving is null)
            return false;

        subtasks.Remove(moving);

        var pos = request.TargetPosition;
        if (pos < 0)
            pos = 0;
        if (pos > subtasks.Count)
            pos = subtasks.Count;

        subtasks.Insert(pos, moving);

        for (int i = 0; i < subtasks.Count; i++)
        {
            subtasks[i].Order = i;
            await _subtaskRepo.UpdateAsync(subtasks[i], cancellationToken);
        }

        return true;
    }
}
