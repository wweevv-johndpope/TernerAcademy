using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class StudentCategoryPreferenceConfiguration : IEntityTypeConfiguration<StudentCategoryPreference>
    {
        public void Configure(EntityTypeBuilder<StudentCategoryPreference> builder)
        {
            builder.ToTable("CategoryPreferences", "Student");
        }
    }
}
