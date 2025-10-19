using Application.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class ReactionEntityConfiguration
    : IEntityTypeConfiguration<ReactionEntity>
{
    public void Configure(EntityTypeBuilder<ReactionEntity> builder)
    {
        builder.HasKey(x=>new {x.UserId, x.EntityType, x.EntityId});
        builder.Property(x => x.EntityType)
            .IsRequired()
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasConversion<string>();
        builder.Property(x=>x.ReactionType)
            .IsRequired()
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasConversion<string>();
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}