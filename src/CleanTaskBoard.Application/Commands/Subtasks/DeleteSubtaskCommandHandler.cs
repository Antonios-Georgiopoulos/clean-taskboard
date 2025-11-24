using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public class DeleteSubtaskCommandHandler : IRequestHandler<DeleteSubtaskCommand, bool>
{
    private readonly ISubtaskRepository _subtaskRepo;

    public DeleteSubtaskCommandHandler(ISubtaskRepository subtaskRepo)
    {
        _subtaskRepo = subtaskRepo;
    }

    public async Task<bool> Handle(
        DeleteSubtaskCommand request,
        CancellationToken cancellationToken
    )
    {
        var subtask = await _subtaskRepo.GetByIdAsync(request.SubtaskId, cancellationToken);
        if (subtask is null)
            return false;

        await _subtaskRepo.DeleteAsync(subtask, cancellationToken);
        return true;
    }
}
