using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

internal class ArgumentConfigurations
    : IEntityTypeConfiguration<Arguments>
{
    public void Configure(EntityTypeBuilder<Arguments> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(25)
            .IsUnicode(false);
        builder.HasIndex(x => x.Code)
            .IsUnique();
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(250);
        builder.Property(x => x.Query)
            .IsRequired();
        builder.HasMany(x => x.NotificationTemplates)
            .WithMany(x => x.Arguments)
            .UsingEntity<Dictionary<Guid, Guid>>(
                "NotificationTemplatesArguments",
                i => i.HasOne<NotificationTemplate>()
                    .WithMany().HasForeignKey("NotificationTemplateId").HasConstraintName("FK_NotificationTemplates_NotificationTemplateId"),
                i => i.HasOne<Arguments>()
                    .WithMany().HasForeignKey("ArgumentsId").HasConstraintName("FK_Arguments_Arguments"),
                j =>
                {
                    j.HasKey("NotificationTemplateId", "ArgumentsId");
                });
    }
}