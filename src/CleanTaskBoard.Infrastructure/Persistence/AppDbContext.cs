using CleanTaskBoard.Domain.Entities;
using CleanTaskBoard.Domain.Entities.Boards;
using CleanTaskBoard.Domain.Entities.Column;
using CleanTaskBoard.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace CleanTaskBoard.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Board> Boards { get; set; } = default!;
    public DbSet<Column> Columns { get; set; } = default!;
    public DbSet<TaskItem> TaskItems { get; set; } = default!;
    public DbSet<Subtask> Subtasks => Set<Subtask>();
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<BoardMembership> BoardMemberships { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Board
        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Name).IsRequired().HasMaxLength(200);
            entity.Property(b => b.OwnerUserId).IsRequired();
        });

        // Column
        modelBuilder.Entity<Column>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);

            entity
                .HasOne<Board>()
                .WithMany()
                .HasForeignKey(c => c.BoardId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // TaskItem
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(300);

            entity
                .HasOne<Column>()
                .WithMany()
                .HasForeignKey(t => t.ColumnId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Subtask
        modelBuilder.Entity<Subtask>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Title).IsRequired();

            entity
                .HasOne<TaskItem>()
                .WithMany()
                .HasForeignKey(s => s.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);

            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
        });

        // BoardMembership
        modelBuilder.Entity<BoardMembership>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.HasIndex(m => new { m.BoardId, m.UserId }).IsUnique();

            entity.Property(m => m.Role).IsRequired();

            entity
                .HasOne(m => m.Board)
                .WithMany()
                .HasForeignKey(m => m.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
