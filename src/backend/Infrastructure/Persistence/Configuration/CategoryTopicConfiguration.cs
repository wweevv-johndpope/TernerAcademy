using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CategoryTopicConfiguration : IEntityTypeConfiguration<CategoryTopic>
    {
        public void Configure(EntityTypeBuilder<CategoryTopic> builder)
        {
            builder.ToTable("Topics", "Category");

            builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.NameNormalize).HasMaxLength(100).IsRequired();

            builder.HasIndex(t => t.Name).IsUnique();
            builder.HasIndex(t => t.NameNormalize).IsUnique();
        }
    }
}
