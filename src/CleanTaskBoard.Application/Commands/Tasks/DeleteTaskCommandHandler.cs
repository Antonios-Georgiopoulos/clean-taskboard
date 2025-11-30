using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly ITaskItemRepository _taskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public DeleteTaskCommandHandler(
        ITaskItemRepository taskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _taskRepo = taskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        await _boardAccessService.EnsureCanEditTask(
            request.Id,
            request.CurrentUserId,
            cancellationToken
        );

        var task = await _taskRepo.GetByIdAsync(request.Id, cancellationToken);
        if (task is null)
            return false;

        await _taskRepo.DeleteAsync(task, cancellationToken);
        return true;
    }
}
