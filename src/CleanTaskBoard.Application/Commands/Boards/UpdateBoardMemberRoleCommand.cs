using CleanTaskBoard.Domain.Entities.Boards;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Boards;

public record UpdateBoardMemberRoleCommand(
    Guid CurrentUserId,
    Guid BoardId,
    Guid MemberUserId,
    BoardRole Role
) : IRequest<bool>;
