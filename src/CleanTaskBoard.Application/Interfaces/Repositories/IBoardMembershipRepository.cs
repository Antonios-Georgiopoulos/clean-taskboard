using CleanTaskBoard.Domain.Entities.Boards;

namespace CleanTaskBoard.Application.Interfaces.Repositories;

public interface IBoardMembershipRepository
{
    Task<Guid> AddAsync(BoardMembership membership, CancellationToken cancellationToken = default);

    Task<BoardMembership?> GetAsync(
        Guid boardId,
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<List<BoardMembership>> GetByBoardAsync(
        Guid boardId,
        CancellationToken cancellationToken = default
    );

    Task UpdateAsync(BoardMembership membership, CancellationToken cancellationToken = default);

    Task RemoveAsync(BoardMembership membership, CancellationToken cancellationToken = default);
}
