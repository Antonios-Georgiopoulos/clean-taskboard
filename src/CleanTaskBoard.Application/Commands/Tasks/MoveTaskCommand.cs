using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record MoveTaskCommand(
    Guid TaskId,
    Guid OwnerUserId,
    Guid TargetColumnId,
    int TargetPosition
) : IRequest<bool>;
