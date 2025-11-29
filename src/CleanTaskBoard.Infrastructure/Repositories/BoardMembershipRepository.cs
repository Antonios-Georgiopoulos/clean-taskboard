using CleanTaskBoard.Application.Interfaces.Repositories;
using CleanTaskBoard.Domain.Entities.Boards;
using CleanTaskBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanTaskBoard.Infrastructure.Repositories;

public class BoardMembershipRepository : IBoardMembershipRepository
{
    private readonly AppDbContext _context;

    public BoardMembershipRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(
        BoardMembership membership,
        CancellationToken cancellationToken = default
    )
    {
        await _context.BoardMemberships.AddAsync(membership, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return membership.Id;
    }

    public async Task<BoardMembership?> GetAsync(
        Guid boardId,
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .BoardMemberships.AsNoTracking()
            .FirstOrDefaultAsync(
                m => m.BoardId == boardId && m.UserId == userId,
                cancellationToken
            );
    }

    public async Task<List<BoardMembership>> GetByBoardAsync(
        Guid boardId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
            .BoardMemberships.AsNoTracking()
            .Where(m => m.BoardId == boardId)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        BoardMembership membership,
        CancellationToken cancellationToken = default
    )
    {
        _context.BoardMemberships.Update(membership);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(
        BoardMembership membership,
        CancellationToken cancellationToken = default
    )
    {
        _context.BoardMemberships.Remove(membership);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
