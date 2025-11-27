using MediatR;

namespace CleanTaskBoard.Application.Commands.Columns;

public record DeleteColumnCommand(Guid Id, Guid OwnerUserId) : IRequest<bool>;
