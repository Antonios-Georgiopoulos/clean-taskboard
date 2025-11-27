using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using CleanTaskBoard.Domain.Enums;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly ITaskItemRepository _taskRepo;
    private readonly IColumnRepository _columnRepo;

    public CreateTaskCommandHandler(ITaskItemRepository taskRepo, IColumnRepository columnRepo)
    {
        _taskRepo = taskRepo;
        _columnRepo = columnRepo;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        // Εξασφάλιση ότι η στήλη ανήκει σε board του χρήστη
        _ =
            await _columnRepo.GetByIdAsync(request.ColumnId, request.OwnerUserId, cancellationToken)
            ?? throw new InvalidOperationException("Column not found or access denied.");
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
