using CleanTaskBoard.Domain.Entities;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Board
        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Name).IsRequired().HasMaxLength(200);
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
    }
}
