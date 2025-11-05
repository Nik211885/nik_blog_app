using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class NotificationConfiguration
    : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Subject)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(x => x.Content)
            .IsRequired();
        builder.HasOne(x => x.UserReceived)
            .WithMany()
            .HasForeignKey(x => x.UserReceivedId);
        builder.HasOne(x => x.UserSendBy)
            .WithMany()
            .HasForeignKey(x => x.UserSendById);
        builder.Property(x => x.UrlNavigation)
            .HasMaxLength(500);
    }
}