using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public record ReorderSubtaskCommand(Guid CurrentUserId, Guid SubtaskId, int TargetPosition)
    : IRequest<bool>;
