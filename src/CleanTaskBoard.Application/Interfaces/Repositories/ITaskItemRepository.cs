namespace CleanTaskBoard.Application.Interfaces.Repositories;

public interface ITaskItemRepository
{
    Task<Guid> AddAsync(TaskItem task, CancellationToken cancellationToken = default);

    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<TaskItem>> GetByColumnIdAsync(
        Guid columnId,
        CancellationToken cancellationToken = default
    );

    Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default);

    Task DeleteAsync(TaskItem task, CancellationToken cancellationToken = default);
}
