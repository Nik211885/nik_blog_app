using System.Text.Json;
using Application.Entities;
using Application.Enums;
using Application.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.Id);
        //  title
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(1000);
        // visibility
        builder.Property(x=>x.Visibility)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();
        // post plug
        builder.Property(x => x.PostSlug)
            .IsRequired()
            .HasMaxLength(100)
            .IsRequired()
            .IsUnicode(false);
        builder.HasIndex(x => x.PostSlug)
            .IsUnique();
        // post subject
        builder.HasMany(x=>x.PostSubjects)
            .WithOne(x=>x.Posts)
            .HasForeignKey(x=>x.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        //  reaction post
        builder.HasMany(x=>x.ReactionPosts)
            .WithOne()
            .HasForeignKey(x=>x.EntityId)
            .OnDelete(DeleteBehavior.Cascade);
        // comment
        builder.HasMany(x=>x.Comments)
            .WithOne(x=>x.Post)
            .HasForeignKey(x=>x.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        // user
        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy);
        // Modified
        builder.HasOne(x => x.ModifiedByUser)
            .WithMany()
            .HasForeignKey(x=>x.ModifiedBy);
        // cout reaction
        builder.Property(x => x.CoutReactions)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasConversion(
                v=>JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v=> JsonSerializer.Deserialize<ICollection<CoutReaction>>(v, (JsonSerializerOptions?)null) ?? new List<CoutReaction>()
            );
    }
}