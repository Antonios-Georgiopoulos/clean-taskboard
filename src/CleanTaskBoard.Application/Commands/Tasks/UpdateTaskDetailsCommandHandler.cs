using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Enums;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class UpdateTaskDetailsCommandHandler : IRequestHandler<UpdateTaskDetailsCommand, bool>
{
    private readonly ITaskItemRepository _taskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public UpdateTaskDetailsCommandHandler(
        ITaskItemRepository taskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _taskRepo = taskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<bool> Handle(
        UpdateTaskDetailsCommand request,
        CancellationToken cancellationToken
    )
    {
        await _boardAccessService.EnsureCanEditTask(
            request.Id,
            request.CurrentUserId,
            cancellationToken
        );

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
