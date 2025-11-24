using MediatR;

namespace CleanTaskBoard.Application.Commands.Subtasks;

public record ReorderSubtaskCommand(Guid SubtaskId, int TargetPosition) : IRequest<bool>;
