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

    public async Task<Subtask?> GetByIdAsync(
        Guid id,
        Guid ownerUserId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Subtasks.AsNoTracking()
            .Where(s => s.Id == id)
            .Join(_context.TaskItems, s => s.TaskItemId, t => t.Id, (s, t) => new { s, t })
            .Join(
                _context.Columns,
                st => st.t.ColumnId,
                c => c.Id,
                (st, c) =>
                    new
                    {
                        st.s,
                        st.t,
                        c,
                    }
            )
            .Join(_context.Boards, stc => stc.c.BoardId, b => b.Id, (stc, b) => new { stc.s, b })
            .Where(s_b => s_b.b.OwnerUserId == ownerUserId)
            .Select(x => x.s)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Subtask>> GetByTaskIdAsync(
        Guid taskItemId,
        Guid ownerUserId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Subtasks.AsNoTracking()
            .Where(s => s.TaskItemId == taskItemId)
            .Join(_context.TaskItems, s => s.TaskItemId, t => t.Id, (s, t) => new { s, t })
            .Join(
                _context.Columns,
                st => st.t.ColumnId,
                c => c.Id,
                (st, c) =>
                    new
                    {
                        st.s,
                        st.t,
                        c,
                    }
            )
            .Join(_context.Boards, stc => stc.c.BoardId, b => b.Id, (stc, b) => new { stc.s, b })
            .Where(s_b => s_b.b.OwnerUserId == ownerUserId)
            .Select(x => x.s)
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
