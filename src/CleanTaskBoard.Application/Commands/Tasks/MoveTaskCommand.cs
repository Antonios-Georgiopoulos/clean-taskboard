using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record MoveTaskCommand(
    Guid CurrentUserId,
    Guid TaskId,
    Guid TargetColumnId,
    int TargetPosition
) : IRequest<bool>;
