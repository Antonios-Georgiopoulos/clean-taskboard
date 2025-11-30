using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public class DeleteSubtaskCommandHandler : IRequestHandler<DeleteSubtaskCommand, bool>
{
    private readonly ISubtaskRepository _subtaskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public DeleteSubtaskCommandHandler(
        ISubtaskRepository subtaskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _subtaskRepo = subtaskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<bool> Handle(
        DeleteSubtaskCommand request,
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
            return false;

        await _subtaskRepo.DeleteAsync(subtask, cancellationToken);
        return true;
    }
}
