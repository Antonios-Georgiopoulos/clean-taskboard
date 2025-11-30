using CleanTaskBoard.Domain.Entities.Boards;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Boards;

public record GetBoardsQuery(Guid CurrentUserId) : IRequest<List<Board>>;
