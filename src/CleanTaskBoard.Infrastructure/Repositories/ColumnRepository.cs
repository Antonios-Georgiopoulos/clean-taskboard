using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using CleanTaskBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanTaskBoard.Infrastructure.Repositories;

public class ColumnRepository : IColumnRepository
{
    private readonly AppDbContext _context;

    public ColumnRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Column column, CancellationToken cancellationToken = default)
    {
        await _context.Columns.AddAsync(column, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return column.Id;
    }

    public async Task<Column?> GetByIdAsync(
        Guid id,
        Guid ownerUserId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Columns.AsNoTracking()
            .Where(c => c.Id == id)
            .Join(_context.Boards, c => c.BoardId, b => b.Id, (c, b) => new { c, b })
            .Where(cb => cb.b.OwnerUserId == ownerUserId)
            .Select(cb => cb.c)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Column>> GetByBoardIdAsync(
        Guid boardId,
        Guid ownerUserId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Columns.AsNoTracking()
            .Where(c => c.BoardId == boardId)
            .Join(_context.Boards, c => c.BoardId, b => b.Id, (c, b) => new { c, b })
            .Where(cb => cb.b.OwnerUserId == ownerUserId)
            .Select(cb => cb.c)
            .OrderBy(c => c.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(Column column, CancellationToken cancellationToken = default)
    {
        _context.Columns.Remove(column);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
