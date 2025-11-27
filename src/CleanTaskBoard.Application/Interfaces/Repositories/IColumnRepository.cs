using CleanTaskBoard.Domain.Entities;

namespace CleanTaskBoard.Application.Interfaces.Repositories;

public interface IColumnRepository
{
    Task<Guid> AddAsync(Column column, CancellationToken cancellationToken = default);

    Task<Column?> GetByIdAsync(
        Guid id,
        Guid ownerUserId,
        CancellationToken cancellationToken = default
    );

    Task<List<Column>> GetByBoardIdAsync(
        Guid boardId,
        Guid ownerUserId,
        CancellationToken cancellationToken = default
    );

    Task DeleteAsync(Column column, CancellationToken cancellationToken = default);
}
