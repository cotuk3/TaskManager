using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastracture.Data;
public class TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<TaskItem> Tasks { get; set; }
    //public override DbSet<User > Users { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(TaskManagerDbContext).Assembly);
    }
}
