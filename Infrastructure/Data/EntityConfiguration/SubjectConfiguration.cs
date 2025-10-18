using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(s => s.Id);
        //name 
        builder.Property(x=>x.Name)
            .HasMaxLength(250)
            .IsRequired();
        // slug
        builder.Property(x=>x.Slug)
            .HasMaxLength(250)
            .IsUnicode()
            .IsRequired();
        builder.HasIndex(x=>x.Slug)
            .IsUnique();
        // lock subject
        builder.OwnsOne(x => x.LockSubject, lockSubject =>
        {
            lockSubject.Property(x => x.ReasonLock)
                .HasMaxLength(500);
        });
        // post subject
        builder.HasMany(x=>x.PostSubjects)
            .WithOne(x=>x.Subjects)
            .HasForeignKey(x=>x.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
        // parent subject
        builder.HasMany(x => x.ChildSubject)
            .WithOne(x => x.SubjectParent)
            .HasForeignKey(x => x.SubjectParentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}