using CleanTaskBoard.Domain.Entities;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Boards;

public record GetBoardByIdQuery(Guid Id) : IRequest<Board?>;
