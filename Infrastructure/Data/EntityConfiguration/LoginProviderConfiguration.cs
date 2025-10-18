using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfiguration;

public class LoginProviderConfiguration : IEntityTypeConfiguration<LoginProvider>
{
    public void Configure(EntityTypeBuilder<LoginProvider> builder)
    {
        builder.HasKey(x=>x.Id);
        builder.Property(x => x.Provider)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(x=>x.Identifier)
            .IsRequired()
            .HasMaxLength(150);
    }
}