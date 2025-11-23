using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Enums;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class UpdateTaskDetailsCommandHandler : IRequestHandler<UpdateTaskDetailsCommand, bool>
{
    private readonly ITaskItemRepository _taskRepo;

    public UpdateTaskDetailsCommandHandler(ITaskItemRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<bool> Handle(
        UpdateTaskDetailsCommand request,
        CancellationToken cancellationToken
    )
    {
        var task = await _taskRepo.GetByIdAsync(request.Id, cancellationToken);
        if (task is null)
            return false;

        task.Title = request.Title;
        task.Description = request.Description;
        task.DueDate = request.DueDate;
        task.Priority = (TaskPriority)request.Priority;

        await _taskRepo.UpdateAsync(task, cancellationToken);

        return true;
    }
}
