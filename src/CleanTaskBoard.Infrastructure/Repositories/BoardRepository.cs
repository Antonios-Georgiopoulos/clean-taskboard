using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities.Boards;
using CleanTaskBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanTaskBoard.Infrastructure.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly AppDbContext _context;

    public BoardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Board board, CancellationToken cancellationToken = default)
    {
        await _context.Boards.AddAsync(board, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return board.Id;
    }

    public async Task<List<Board>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Boards.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Board?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Boards.AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }
}
