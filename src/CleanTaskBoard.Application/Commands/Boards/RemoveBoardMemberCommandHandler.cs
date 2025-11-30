using CleanTaskBoard.Application.Common.Exceptions;
using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Boards;

public class RemoveBoardMemberCommandHandler : IRequestHandler<RemoveBoardMemberCommand, bool>
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardMembershipRepository _membershipRepository;
    private readonly IBoardAccessService _boardAccessService;

    public RemoveBoardMemberCommandHandler(
        IBoardRepository boardRepository,
        IBoardMembershipRepository membershipRepository,
        IBoardAccessService boardAccessService
    )
    {
        _boardRepository = boardRepository;
        _membershipRepository = membershipRepository;
        _boardAccessService = boardAccessService;
    }

    public async Task<bool> Handle(
        RemoveBoardMemberCommand request,
        CancellationToken cancellationToken
    )
    {
        await _boardAccessService.EnsureCanManageMembership(
            request.BoardId,
            request.CurrentUserId,
            cancellationToken
        );

        var board = await _boardRepository.GetByIdAsync(request.BoardId, cancellationToken);

        if (board is null)
        {
            throw new NotFoundException("Board", request.BoardId);
        }

        var membership = await _membershipRepository.GetAsync(
            request.BoardId,
            request.MemberUserId,
            cancellationToken
        );

        if (membership is null)
        {
            throw new NotFoundException(
                "BoardMembership",
                $"{request.BoardId}:{request.MemberUserId}"
            );
        }

        await _membershipRepository.RemoveAsync(membership, cancellationToken);
        return true;
    }
}
