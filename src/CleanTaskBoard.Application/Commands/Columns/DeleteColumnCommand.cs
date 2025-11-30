using MediatR;

namespace CleanTaskBoard.Application.Commands.Columns;

public record DeleteColumnCommand(Guid CurrentUserId, Guid Id) : IRequest<bool>;
