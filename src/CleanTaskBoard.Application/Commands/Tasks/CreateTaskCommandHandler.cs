using CleanTaskBoard.Application.Common.Exceptions;
using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Enums;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly ITaskItemRepository _taskRepo;
    private readonly IColumnRepository _columnRepo;
    private readonly IBoardAccessService _boardAccessService;

    public CreateTaskCommandHandler(
        ITaskItemRepository taskRepo,
        IColumnRepository columnRepo,
        IBoardAccessService boardAccessService
    )
    {
        _taskRepo = taskRepo;
        _columnRepo = columnRepo;
        _boardAccessService = boardAccessService;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var column = await _columnRepo.GetByIdAsync(request.ColumnId, cancellationToken);

        if (column is null)
        {
            throw new NotFoundException("Column", request.ColumnId);
        }

        await _boardAccessService.EnsureCanEditTasksForColumn(
            column.Id,
            request.CurrentUserId,
            cancellationToken
        );

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
