using CleanTaskBoard.Domain.Entities.Boards;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Boards;

public record GetBoardsQuery() : IRequest<List<Board>>;
