using CleanTaskBoard.Domain.Entities.Boards;

namespace CleanTaskBoard.Application.Interfaces.Repositories;

public interface IBoardRepository
{
    Task<Guid> AddAsync(Board board, CancellationToken cancellationToken = default);

    Task<Board?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Board>> GetByOwnerAsync(
        Guid ownerUserId,
        CancellationToken cancellationToken = default
    );
}
