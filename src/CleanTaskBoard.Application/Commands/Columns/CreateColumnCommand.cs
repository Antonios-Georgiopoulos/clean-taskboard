using MediatR;

namespace CleanTaskBoard.Application.Commands.Columns;

public record CreateColumnCommand(Guid OwnerUserId, Guid BoardId, string Name, int Order)
    : IRequest<Guid>;
