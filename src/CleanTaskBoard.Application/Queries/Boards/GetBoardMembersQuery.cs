using CleanTaskBoard.Application.Models.Boards;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Boards;

public record GetBoardMembersQuery(Guid CurrentUserId, Guid BoardId)
    : IRequest<List<BoardMemberDto>>;
