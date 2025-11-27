using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
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
        Guid ownerUserId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .TaskItems.AsNoTracking()
            .Where(t => t.Id == id)
            .Join(_context.Columns, t => t.ColumnId, c => c.Id, (t, c) => new { t, c })
            .Join(_context.Boards, tc => tc.c.BoardId, b => b.Id, (tc, b) => new { tc.t, b })
            .Where(tcb => tcb.b.OwnerUserId == ownerUserId)
            .Select(x => x.t)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<TaskItem>> GetByColumnIdAsync(
        Guid columnId,
        Guid ownerUserId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .TaskItems.AsNoTracking()
            .Where(t => t.ColumnId == columnId)
            .Join(_context.Columns, t => t.ColumnId, c => c.Id, (t, c) => new { t, c })
            .Join(_context.Boards, tc => tc.c.BoardId, b => b.Id, (tc, b) => new { tc.t, b })
            .Where(tcb => tcb.b.OwnerUserId == ownerUserId)
            .Select(x => x.t)
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
