using CleanTaskBoard.Domain.Entities;

namespace CleanTaskBoard.Application.Interfaces.Repositories;

public interface ISubtaskRepository
{
    Task<Guid> AddAsync(Subtask subtask, CancellationToken cancellationToken = default);

    Task<Subtask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Subtask>> GetByTaskIdAsync(
        Guid taskItemId,
        CancellationToken cancellationToken = default
    );

    Task UpdateAsync(Subtask subtask, CancellationToken cancellationToken = default);

    Task DeleteAsync(Subtask subtask, CancellationToken cancellationToken = default);
}
