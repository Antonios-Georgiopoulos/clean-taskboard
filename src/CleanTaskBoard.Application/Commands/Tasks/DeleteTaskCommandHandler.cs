using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly ITaskItemRepository _taskRepo;

    public DeleteTaskCommandHandler(ITaskItemRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepo.GetByIdAsync(request.Id, request.OwnerUserId, cancellationToken);

        if (task is null)
            return false;

        await _taskRepo.DeleteAsync(task, cancellationToken);
        return true;
    }
}
