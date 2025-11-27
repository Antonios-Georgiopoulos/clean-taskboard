using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record CreateTaskCommand(
    Guid OwnerUserId,
    Guid ColumnId,
    string Title,
    string? Description,
    DateTime? DueDate,
    int Priority
) : IRequest<Guid>;
