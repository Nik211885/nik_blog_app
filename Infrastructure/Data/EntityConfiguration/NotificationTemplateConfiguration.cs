using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class NotificationTemplateConfiguration
    : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.Subject)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(x => x.NotificationChanel)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsUnicode(false);
        builder.Property(x => x.NotificationServicesType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsUnicode(false);
    }
}