using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public class CreateSubtaskCommandHandler : IRequestHandler<CreateSubtaskCommand, Guid>
{
    private readonly ISubtaskRepository _subtaskRepo;

    public CreateSubtaskCommandHandler(ISubtaskRepository subtaskRepo)
    {
        _subtaskRepo = subtaskRepo;
    }

    public async Task<Guid> Handle(
        CreateSubtaskCommand request,
        CancellationToken cancellationToken
    )
    {
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
