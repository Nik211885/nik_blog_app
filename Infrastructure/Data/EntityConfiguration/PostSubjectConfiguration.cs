using Application.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class PostSubjectConfiguration : IEntityTypeConfiguration<PostSubject>
{
    public void Configure(EntityTypeBuilder<PostSubject> builder)
    {
        builder.HasIndex(x=> new {x.PostId, x.SubjectId});
    }
}