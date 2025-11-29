using CleanTaskBoard.Application.Common.Exceptions;
using CleanTaskBoard.Application.Interfaces.Repositories;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Boards;

public class RemoveBoardMemberCommandHandler : IRequestHandler<RemoveBoardMemberCommand, bool>
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardMembershipRepository _membershipRepository;

    public RemoveBoardMemberCommandHandler(
        IBoardRepository boardRepository,
        IBoardMembershipRepository membershipRepository
    )
    {
        _boardRepository = boardRepository;
        _membershipRepository = membershipRepository;
    }

    public async Task<bool> Handle(
        RemoveBoardMemberCommand request,
        CancellationToken cancellationToken
    )
    {
        var board = await _boardRepository.GetByIdAsync(
            request.BoardId,
            request.CurrentUserId,
            cancellationToken
        );

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
