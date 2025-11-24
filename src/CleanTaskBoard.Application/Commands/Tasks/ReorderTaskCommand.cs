using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record ReorderTaskCommand(Guid TaskId, int TargetPosition) : IRequest<bool>;
