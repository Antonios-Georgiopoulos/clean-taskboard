using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, bool>
{
    private readonly ITaskItemRepository _taskRepo;

    public CompleteTaskCommandHandler(ITaskItemRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<bool> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepo.GetByIdAsync(
            request.TaskId,
            request.OwnerUserId,
            cancellationToken
        );

        if (task is null)
            return false;

        task.IsCompleted = !task.IsCompleted;

        await _taskRepo.UpdateAsync(task, cancellationToken);

        return true;
    }
}
