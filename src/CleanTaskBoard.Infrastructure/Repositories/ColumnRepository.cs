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

    public async Task<Column?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Columns.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<List<Column>> GetByBoardIdAsync(
        Guid boardId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .Columns.AsNoTracking()
            .Where(c => c.BoardId == boardId)
            .OrderBy(c => c.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(Column column, CancellationToken cancellationToken = default)
    {
        _context.Columns.Remove(column);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
