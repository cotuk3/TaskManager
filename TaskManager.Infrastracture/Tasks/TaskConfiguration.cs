using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastracture.Tasks;
internal sealed class TaskConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever();
        builder.Property(e => e.UserId)
            .IsRequired();
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.OwnsOne(e => e.DueDate, dd =>
        {
            dd.Property(p => p.Value)
              .HasColumnName("DueDate")
              .IsRequired();
        });

        builder.Property(e => e.Labels)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        builder.Property(e => e.IsCompleted)
            .IsRequired();
        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.CompletedAt);
        builder.Property(e => e.Priority)
            .IsRequired();

        

        builder.HasAnnotation("Relational:MigrationHistoryTable", "__EFMigrationsHistory_Task");}
}
