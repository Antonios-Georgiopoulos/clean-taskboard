using MediatR;

namespace CleanTaskBoard.Application.Commands.Tasks;

public record DeleteTaskCommand(Guid CurrentUserId, Guid Id) : IRequest<bool>;
