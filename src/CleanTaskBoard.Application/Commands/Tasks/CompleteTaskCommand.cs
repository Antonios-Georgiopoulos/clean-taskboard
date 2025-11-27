using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record CompleteTaskCommand(Guid TaskId, Guid OwnerUserId) : IRequest<bool>;
