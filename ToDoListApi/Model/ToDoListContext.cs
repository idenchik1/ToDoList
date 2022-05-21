using Microsoft.EntityFrameworkCore;
using ToDoListApi.Entities;
using Task = ToDoListApi.Entities.Task;

namespace ToDoListApi.Model;

public class ToDoListContext : DbContext
{
    public ToDoListContext()
    {
    }

    public ToDoListContext(DbContextOptions<ToDoListContext> options)
        : base(options)
    {
    }

    public virtual DbSet<List> Lists { get; set; } = null!;
    public virtual DbSet<Task> Tasks { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<List>(entity =>
        {
            entity.HasOne(d => d.ListOwnerNavigation)
                .WithMany(p => p.Lists)
                .HasForeignKey(d => d.ListOwner)
                .HasConstraintName("ListOwner");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasOne(d => d.TaskListNavigation)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TaskList)
                .HasConstraintName("TaskList");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    private void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        throw new NotImplementedException();
    }
}