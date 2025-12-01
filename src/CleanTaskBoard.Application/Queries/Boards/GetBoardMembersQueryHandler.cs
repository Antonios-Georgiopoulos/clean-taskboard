using CleanTaskBoard.Application.Common.Exceptions;
using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Application.Interfaces.Services;
using CleanTaskBoard.Application.Models.Boards;
using CleanTaskBoard.Domain.Entities.Boards;
using MediatR;

namespace CleanTaskBoard.Application.Queries.Boards;

public class GetBoardMembersQueryHandler
    : IRequestHandler<GetBoardMembersQuery, List<BoardMemberDto>>
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardMembershipRepository _membershipRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBoardAccessService _boardAccessService;

    public GetBoardMembersQueryHandler(
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

    public async Task<List<BoardMemberDto>> Handle(
        GetBoardMembersQuery request,
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

        var memberships = await _membershipRepository.GetByBoardAsync(
            request.BoardId,
            cancellationToken
        );

        var userIds = memberships.Select(m => m.UserId).Distinct().ToList();

        var users = new List<Domain.Entities.Users.User>();
        foreach (var userId in userIds.ToList())
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is not null)
            {
                users.Add(user);
            }
        }

        var result = (
            from m in memberships
            join u in users on m.UserId equals u.Id
            select new BoardMemberDto(m.UserId, u.Email, m.Role.ToString())
        ).ToList();

        // Owner as "virtual member (if not exists in table)
        var ownerUser = await _userRepository.GetByIdAsync(board.OwnerUserId, cancellationToken);

        if (ownerUser is not null && !result.Any(r => r.UserId == board.OwnerUserId))
        {
            result.Insert(
                0,
                new BoardMemberDto(board.OwnerUserId, ownerUser.Email, BoardRole.Owner.ToString())
            );
        }

        return result;
    }
}
