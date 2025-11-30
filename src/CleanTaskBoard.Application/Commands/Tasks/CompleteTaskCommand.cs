using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record CompleteTaskCommand(Guid CurrentUserId, Guid TaskId) : IRequest<bool>;
