using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record DeleteTaskCommand(Guid Id) : IRequest<bool>;
