using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.ToTable("Instructors", "Instructor");

            builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Email).HasMaxLength(100).IsRequired();
            builder.Property(t => t.EmailNormalize).HasMaxLength(100).IsRequired();

            builder.Property(t => t.CompanyName).HasMaxLength(256);
            builder.Property(t => t.Bio).HasMaxLength(2048);
            builder.Property(t => t.ProfilePictureFilename).HasMaxLength(256);
            builder.Property(t => t.WalletAddress).HasMaxLength(64);

            builder.HasIndex(t => t.Email).IsUnique();
            builder.HasIndex(t => t.EmailNormalize).IsUnique();
        }
    }
}
