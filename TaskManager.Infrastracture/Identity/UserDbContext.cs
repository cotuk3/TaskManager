using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastracture.Identity;
public class UserDbContext(DbContextOptions<UserDbContext> options) :
    IdentityDbContext<User>(options)
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
    }
}
