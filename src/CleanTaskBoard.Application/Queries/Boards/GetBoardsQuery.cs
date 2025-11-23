using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Boards;

public record GetBoardsQuery() : IRequest<List<Board>>;
