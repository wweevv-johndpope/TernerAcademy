using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories", "Category");

            builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.NameNormalize).HasMaxLength(100).IsRequired();

            builder.HasIndex(t => t.Name).IsUnique();
            builder.HasIndex(t => t.NameNormalize).IsUnique();
        }
    }
}
