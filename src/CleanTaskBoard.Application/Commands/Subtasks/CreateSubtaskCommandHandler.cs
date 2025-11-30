using CleanTaskBoard.Application.Common.Exceptions;
using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public class CreateSubtaskCommandHandler : IRequestHandler<CreateSubtaskCommand, Guid>
{
    private readonly ISubtaskRepository _subtaskRepo;
    private readonly ITaskItemRepository _taskRepo;
    private readonly IBoardAccessService _boardAccessService;

    public CreateSubtaskCommandHandler(
        ISubtaskRepository subtaskRepo,
        ITaskItemRepository taskRepo,
        IBoardAccessService boardAccessService
    )
    {
        _subtaskRepo = subtaskRepo;
        _taskRepo = taskRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<Guid> Handle(
        CreateSubtaskCommand request,
        CancellationToken cancellationToken
    )
    {
        var task = await _taskRepo.GetByIdAsync(request.TaskItemId, cancellationToken);

        if (task is null)
        {
            throw new NotFoundException("TaskItem", request.TaskItemId);
        }

        await _boardAccessService.EnsureCanEditSubtask(
            subtaskId: Guid.Empty, // ιδανικά EnsureCanEditSubtasksForTask(task.Id,...)
            userId: request.CurrentUserId,
            cancellationToken
        );

        var subtask = new Subtask
        {
            Id = Guid.NewGuid(),
            TaskItemId = request.TaskItemId,
            Title = request.Title,
            IsCompleted = false,
            Order = request.Order,
        };

        return await _subtaskRepo.AddAsync(subtask, cancellationToken);
    }
}
