using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities;
using CleanTaskBoard.Infrastructure.Persistence;

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
}
