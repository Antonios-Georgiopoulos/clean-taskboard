using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using CleanTaskBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanTaskBoard.Infrastructure.Repositories;

public class SubtaskRepository : ISubtaskRepository
{
    private readonly AppDbContext _context;

    public SubtaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Subtask subtask, CancellationToken cancellationToken = default)
    {
        await _context.Subtasks.AddAsync(subtask, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return subtask.Id;
    }

    public async Task<Subtask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Subtasks.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<List<Subtask>> GetByTaskIdAsync(
        Guid taskItemId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Subtasks.AsNoTracking()
            .Where(s => s.TaskItemId == taskItemId)
            .OrderBy(s => s.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Subtask subtask, CancellationToken cancellationToken = default)
    {
        _context.Subtasks.Update(subtask);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Subtask subtask, CancellationToken cancellationToken = default)
    {
        _context.Subtasks.Remove(subtask);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
