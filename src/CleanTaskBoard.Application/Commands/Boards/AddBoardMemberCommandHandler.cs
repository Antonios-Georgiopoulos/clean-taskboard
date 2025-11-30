using CleanTaskBoard.Application.Common.Exceptions;
using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Domain.Entities.Boards;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Boards;

public class AddBoardMemberCommandHandler : IRequestHandler<AddBoardMemberCommand, bool>
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardMembershipRepository _membershipRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBoardAccessService _boardAccessService;

    public AddBoardMemberCommandHandler(
        IBoardRepository boardRepository,
        IBoardMembershipRepository membershipRepository,
        IUserRepository userRepository,
        IBoardAccessService boardAccessService
    )
    {
        _boardRepository = boardRepository;
        _membershipRepository = membershipRepository;
        _userRepository = userRepository;
        _boardAccessService = boardAccessService;
    }

    public async Task<bool> Handle(
        AddBoardMemberCommand request,
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

        var user = await _userRepository.GetByIdAsync(request.MemberUserId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException("User", request.MemberUserId);
        }

        var existing = await _membershipRepository.GetAsync(
            request.BoardId,
            request.MemberUserId,
            cancellationToken
        );

        if (existing is not null)
        {
            existing.Role = request.Role;
            await _membershipRepository.UpdateAsync(existing, cancellationToken);
            return true;
        }

        var membership = new BoardMembership
        {
            Id = Guid.NewGuid(),
            BoardId = request.BoardId,
            UserId = request.MemberUserId,
            Role = request.Role,
        };

        await _membershipRepository.AddAsync(membership, cancellationToken);
        return true;
    }
}
