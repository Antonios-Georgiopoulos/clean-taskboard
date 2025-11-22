using MediatR;

namespace CleanTaskBoard.Application.Commands.Boards;

public record CreateBoardCommand(string Name) : IRequest<Guid>;
