using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public record ReorderSubtaskCommand(Guid SubtaskId, Guid OwnerUserId, int TargetPosition)
    : IRequest<bool>;
