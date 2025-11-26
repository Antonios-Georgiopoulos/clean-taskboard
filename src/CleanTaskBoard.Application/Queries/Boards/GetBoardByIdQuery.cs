using CleanTaskBoard.Domain.Entities.Boards;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Boards;

public record GetBoardByIdQuery(Guid Id, Guid OwnerUserId) : IRequest<Board?>;
