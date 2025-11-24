using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record CompleteTaskCommand(Guid TaskId) : IRequest<bool>;
