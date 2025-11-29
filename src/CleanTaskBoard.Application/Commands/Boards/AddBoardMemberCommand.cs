using CleanTaskBoard.Domain.Entities.Boards;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Boards;

public record AddBoardMemberCommand(
    Guid CurrentUserId, // caller (should be Owner)
    Guid BoardId,
    Guid MemberUserId,
    BoardRole Role
) : IRequest<bool>;
