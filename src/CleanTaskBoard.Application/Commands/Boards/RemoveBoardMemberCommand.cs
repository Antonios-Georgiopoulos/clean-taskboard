using MediatR;

namespace CleanTaskBoard.Application.Commands.Boards;

public record RemoveBoardMemberCommand(Guid CurrentUserId, Guid BoardId, Guid MemberUserId)
    : IRequest<bool>;
