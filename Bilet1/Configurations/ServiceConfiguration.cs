using Bilet1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bilet1.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
       builder.Property(x=>x.Title).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(256);
        builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(256);

        builder.HasOne(x=>x.category).WithMany(x=>x.Services).HasForeignKey(x=>x.CategoryId).HasPrincipalKey(x=>x.Id);
    }
}
