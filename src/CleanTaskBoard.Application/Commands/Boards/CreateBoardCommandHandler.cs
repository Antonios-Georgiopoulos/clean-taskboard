using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities.Boards;
using MediatR;

namespace CleanTaskBoard.Application.Commands.Boards;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, Guid>
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardMembershipRepository _membershipRepository;

    public CreateBoardCommandHandler(
        IBoardRepository boardRepository,
        IBoardMembershipRepository membershipRepository
    )
    {
        _boardRepository = boardRepository;
        _membershipRepository = membershipRepository;
    }

    public async Task<Guid> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var board = new Board
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            OwnerUserId = request.OwnerUserId,
        };

        var boardId = await _boardRepository.AddAsync(board, cancellationToken);

        var membership = new BoardMembership
        {
            Id = Guid.NewGuid(),
            BoardId = boardId,
            UserId = request.OwnerUserId,
            Role = BoardRole.Owner,
        };

        await _membershipRepository.AddAsync(membership, cancellationToken);

        return boardId;
    }
}
