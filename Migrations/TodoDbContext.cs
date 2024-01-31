using challegeToDo_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace challegeToDo_Backend.Migrations;
public class TodoDbContext : DbContext
{
    public  DbSet <user> users {get; set;}
    public DbSet<task> tasks {get; set;}

    public TodoDbContext(DbContextOptions<TodoDbContext> options)
    :base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<user>()
        .HasMany<task>(u => u.tasks)
        .WithOne(t => t.user)
        .HasForeignKey(u => u.userId);

        modelBuilder.Entity<task>(entity => 
        {
           entity.HasKey(e => e.taskId);
           entity.Property(e => e.taskName).IsRequired ();
           entity.HasIndex(e => e.userId);
           entity.Property(e => e.userId).IsRequired();
           entity.Property(e => e.completed).IsRequired();
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("datetime()");
        });

        modelBuilder.Entity<user>(entity => {
            entity.HasKey(e => e.userId);
            entity.Property(e=>e.names).IsRequired();
            entity.Property(e =>e.email).IsRequired();
            entity.HasIndex(x => x.email).IsUnique();
            entity.Property(e=>e.password).IsRequired();
        });
    }
}