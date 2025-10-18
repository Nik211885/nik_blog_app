using System.Text.Json;
using Application.Entities;
using Application.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);
        // content comment
        builder.Property(x=>x.ContentComment)
            .IsRequired();
        // user
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x=>x.UserId);
        // comment child
        builder.HasMany(x=>x.CommentChilds)
            .WithOne(x=>x.CommentParent)
            .HasForeignKey(x=>x.CommentParentId);
        // reaction comment
        builder.HasMany(x=>x.ReactionComments)
            .WithOne(x=>x.Entity)
            .HasForeignKey(x=>x.EntityId)
            .OnDelete(DeleteBehavior.Cascade);
        // cout reaction
        builder.Property(x => x.CoutReactions)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasConversion( 
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<ICollection<CoutReaction>>(v, (JsonSerializerOptions?)null) ?? new List<CoutReaction>()
            );
    }
}