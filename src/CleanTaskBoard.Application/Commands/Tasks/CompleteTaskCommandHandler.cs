using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, bool>
{
    private readonly ITaskItemRepository _taskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public CompleteTaskCommandHandler(
        ITaskItemRepository taskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _taskRepo = taskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<bool> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        await _boardAccessService.EnsureCanEditTask(
            request.TaskId,
            request.CurrentUserId,
            cancellationToken
        );

        var task = await _taskRepo.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null)
            return false;

        task.IsCompleted = !task.IsCompleted;

        await _taskRepo.UpdateAsync(task, cancellationToken);

        return true;
    }
}
