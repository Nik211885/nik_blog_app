using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class MailInfoConfiguration
    : IEntityTypeConfiguration<MailInfo>
{
    public void Configure(EntityTypeBuilder<MailInfo> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(x => x.Host)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.EmailId)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(150)
            .IsUnicode(false);
        builder.HasMany(x => x.NotificationTemplates)
            .WithOne(x => x.MailInfo)
            .HasForeignKey(x => x.MailInfoId);
    }
}