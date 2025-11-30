using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanTaskBoard.Infrastructure.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly AppDbContext _context;

    public TaskItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        await _context.TaskItems.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return task.Id;
    }

    public async Task<TaskItem?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .TaskItems.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<List<TaskItem>> GetByColumnIdAsync(
        Guid columnId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .TaskItems.AsNoTracking()
            .Where(t => t.ColumnId == columnId)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        _context.TaskItems.Update(task);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
