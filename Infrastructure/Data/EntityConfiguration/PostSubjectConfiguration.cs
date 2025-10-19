using Application.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class PostSubjectConfiguration : IEntityTypeConfiguration<PostSubject>
{
    public void Configure(EntityTypeBuilder<PostSubject> builder)
    {
        builder.HasKey(x=> new {x.PostId, x.SubjectId});
        builder.HasOne(x=>x.Posts)
            .WithMany(x=>x.PostSubjects)
            .HasForeignKey(x=>x.PostId);
        builder.HasOne(x=>x.Subjects)
            .WithMany(x=>x.PostSubjects)
            .HasForeignKey(x=>x.SubjectId);
    }
}