using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public class CompleteSubtaskCommandHandler : IRequestHandler<CompleteSubtaskCommand, bool>
{
    private readonly ISubtaskRepository _subtaskRepo;

    public CompleteSubtaskCommandHandler(ISubtaskRepository subtaskRepo)
    {
        _subtaskRepo = subtaskRepo;
    }

    public async Task<bool> Handle(
        CompleteSubtaskCommand request,
        CancellationToken cancellationToken
    )
    {
        var subtask = await _subtaskRepo.GetByIdAsync(
            request.SubtaskId,
            request.OwnerUserId,
            cancellationToken
        );
        if (subtask is null)
            return false;

        subtask.IsCompleted = !subtask.IsCompleted;
        await _subtaskRepo.UpdateAsync(subtask, cancellationToken);

        return true;
    }
}
