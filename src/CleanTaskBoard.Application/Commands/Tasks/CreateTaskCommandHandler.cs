using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Enums;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly ITaskItemRepository _taskRepo;

    public CreateTaskCommandHandler(ITaskItemRepository taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            ColumnId = request.ColumnId,
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            DueDate = request.DueDate,
            Priority = (TaskPriority)request.Priority,
            IsCompleted = false,
        };

        return await _taskRepo.AddAsync(task, cancellationToken);
    }
}
