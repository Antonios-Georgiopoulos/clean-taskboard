using CleanTaskBoard.Domain.Entities;

namespace CleanTaskBoard.Application.Interfaces.Repositories;

public interface IBoardRepository
{
    Task<Guid> AddAsync(Board board, CancellationToken cancellationToken = default);
}
