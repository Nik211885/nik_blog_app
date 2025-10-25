using Application.Entities;
using Application.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        // User name 
        builder.Property(x => x.UserName)
            .HasMaxLength(100)
            .IsRequired(false);
        builder.HasIndex(x=>x.UserName)
            .IsUnique();
        // Email
        builder.Property(x => x.Email)
            .HasMaxLength(150);
        builder.HasIndex(x => x.Email);
        // Password
        builder.Property(x=>x.Password)
            .HasMaxLength(200)
            .IsUnicode(false);
        //  first name
        builder.Property(x => x.FirstName)
            .HasMaxLength(100);
        // last name 
        builder.Property(x => x.LastName)
            .HasMaxLength(100);
        // full name
        builder.Property(x => x.FullName)
            .HasMaxLength(200);
        // phone
        builder.Property(x=>x.PhoneNumber)
            .HasMaxLength(10)
            .IsUnicode(false);
        // avatar
        builder.Property(x=>x.Avatar)
            .HasMaxLength(200)
            .IsUnicode(false);
        // role 
        builder.Property(x=>x.Role)
            .HasConversion<string>()
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(10);
        // user slug
        builder.Property(x => x.UserCvSlug)
            .HasMaxLength(100)
            .IsUnicode(false)
            .IsRequired();
        builder.HasIndex(x => x.UserCvSlug)
            .IsUnique();
        // login provider
        builder.HasMany(x=>x.LoginProviders)
            .WithOne(x=>x.User)
            .HasForeignKey(x=>x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        //lock account
        builder.OwnsOne(x => x.LockAccount, lockAccount =>
        {
            lockAccount.Property(x => x.ReasonLock)
                .HasMaxLength(500);
        });
        // user sub domain
        builder.OwnsOne(x => x.UserSubDomain, userSubDomain =>
        {
            // slug for sub domain
            userSubDomain.Property(x => x.SubDomainBlogSlug)
                .HasMaxLength(150)
                .IsUnicode(false);
            userSubDomain.HasIndex(x=>x.SubDomainBlogSlug)
                .IsUnique();
            // subject domain
            userSubDomain.OwnsOne(x => x.LockSubDomain, lockSubDomain =>
            {
                lockSubDomain.Property(x=>x.ReasonLock)
                    .HasMaxLength(500);
            });
        });
        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy);
        // Modified
        builder.HasOne(x => x.ModifiedByUser)
            .WithMany()
            .HasForeignKey(x=>x.ModifiedBy);
        builder.HasMany(x => x.Subjects)
            .WithOne(x => x.CreatedByUser)
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Cascade);
    }
}