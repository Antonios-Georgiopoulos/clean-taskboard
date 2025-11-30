using CleanTaskBoard.Application.Common.Exceptions;
using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class ReorderTaskCommandHandler : IRequestHandler<ReorderTaskCommand, bool>
{
    private readonly ITaskItemRepository _taskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public ReorderTaskCommandHandler(
        ITaskItemRepository taskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _taskRepo = taskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<bool> Handle(ReorderTaskCommand request, CancellationToken cancellationToken)
    {
        await _boardAccessService.EnsureCanEditTask(
            request.TaskId,
            request.CurrentUserId,
            cancellationToken
        );

        var task = await _taskRepo.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null)
            throw new NotFoundException("TaskItem", request.TaskId);

        var tasks = await _taskRepo.GetByColumnIdAsync(task.ColumnId, cancellationToken);

        tasks = tasks.OrderBy(t => t.Order).ToList();

        var moving = tasks.FirstOrDefault(t => t.Id == request.TaskId);
        if (moving is null)
            return false;

        tasks.Remove(moving);

        var pos = request.TargetPosition;
        if (pos < 0)
            pos = 0;
        if (pos > tasks.Count)
            pos = tasks.Count;

        tasks.Insert(pos, moving);

        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].Order = i;
            await _taskRepo.UpdateAsync(tasks[i], cancellationToken);
        }

        return true;
    }
}
