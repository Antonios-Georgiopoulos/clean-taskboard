using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public class CreateSubtaskCommandHandler : IRequestHandler<CreateSubtaskCommand, Guid>
{
    private readonly ISubtaskRepository _subtaskRepo;
    private readonly ITaskItemRepository _taskRepo;

    public CreateSubtaskCommandHandler(ISubtaskRepository subtaskRepo, ITaskItemRepository taskRepo)
    {
        _subtaskRepo = subtaskRepo;
        _taskRepo = taskRepo;
    }

    public async Task<Guid> Handle(
        CreateSubtaskCommand request,
        CancellationToken cancellationToken
    )
    {
        // Task πρέπει να ανήκει στον χρήστη
        var task = await _taskRepo.GetByIdAsync(
            request.TaskItemId,
            request.OwnerUserId,
            cancellationToken
        );
        if (task is null)
        {
            throw new InvalidOperationException("Task not found or access denied.");
        }

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
