using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record ReorderTaskCommand(Guid TaskId, Guid OwnerUserId, int TargetPosition)
    : IRequest<bool>;
