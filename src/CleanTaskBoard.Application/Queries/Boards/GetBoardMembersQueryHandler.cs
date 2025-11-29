using CleanTaskBoard.Application.Common.Exceptions;
using CleanTaskBoard.Application.Interfaces.Repositories;
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

    public GetBoardMembersQueryHandler(
        IBoardRepository boardRepository,
        IBoardMembershipRepository membershipRepository,
        IUserRepository userRepository
    )
    {
        _boardRepository = boardRepository;
        _membershipRepository = membershipRepository;
        _userRepository = userRepository;
    }

    public async Task<List<BoardMemberDto>> Handle(
        GetBoardMembersQuery request,
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

        // Μέλη από τον πίνακα BoardMemberships
        var memberships = await _membershipRepository.GetByBoardAsync(
            request.BoardId,
            cancellationToken
        );

        var userIds = memberships.Select(m => m.UserId).Distinct().ToList();

        var users = new List<Domain.Entities.Users.User>();
        foreach (var userId in userIds)
        {
            users.Add(
                await _userRepository.GetByIdAsync(userId, cancellationToken)
                    ?? throw new NotFoundException("User", userId)
            );
        }

        var result = (
            from m in memberships
            join u in users on m.UserId equals u.Id
            select new BoardMemberDto(m.UserId, u.Email, m.Role.ToString())
        ).ToList();

        // Προσθέτουμε explicit και τον Owner σαν "virtual member"
        var ownerUser = await _userRepository.GetByIdAsync(board.OwnerUserId, cancellationToken);
        if (ownerUser is not null)
        {
            // Αν δεν υπάρχει ήδη στα memberships
            if (!result.Any(r => r.UserId == board.OwnerUserId))
            {
                result.Insert(
                    0,
                    new BoardMemberDto(
                        board.OwnerUserId,
                        ownerUser.Email,
                        BoardRole.Owner.ToString()
                    )
                );
            }
        }

        return result;
    }
}
