using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record CreateTaskCommand(
    Guid CurrentUserId,
    Guid ColumnId,
    string Title,
    string? Description,
    DateTime? DueDate,
    int Priority
) : IRequest<Guid>;
