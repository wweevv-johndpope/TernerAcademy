using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class InstructorCommunityConfiguration : IEntityTypeConfiguration<InstructorCommunity>
    {
        public void Configure(EntityTypeBuilder<InstructorCommunity> builder)
        {
            builder.ToTable("Communities", "Instructor");

            builder.Property(t => t.Platform).HasMaxLength(100).IsRequired();
            builder.Property(t => t.HandleName).HasMaxLength(100).IsRequired();
        }
    }
}
