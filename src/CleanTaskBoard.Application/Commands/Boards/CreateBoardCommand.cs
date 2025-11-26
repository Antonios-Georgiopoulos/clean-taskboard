using MediatR;

namespace CleanTaskBoard.Application.Commands.Boards;

public record CreateBoardCommand(Guid OwnerUserId, string Name) : IRequest<Guid>;
