using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastracture.Users;
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever();
        builder.Property(e => e.UserName)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(e => e.NormalizedUserName)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(e => e.Email)
            .HasMaxLength(256);
        builder.Property(e => e.NormalizedEmail)
            .HasMaxLength(256);
        builder.HasIndex(e => e.NormalizedUserName).IsUnique();
        builder.HasIndex(e => e.NormalizedEmail).IsUnique();
    }
}
