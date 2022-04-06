using Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Views
{
    public class StudentCourseSubscriptionPurchaseViewConfiguration : IEntityTypeConfiguration<StudentCourseSubscriptionPurchaseViewItem>
    {
        public void Configure(EntityTypeBuilder<StudentCourseSubscriptionPurchaseViewItem> builder)
        {
            builder.ToView("StudentCourseSubscriptionPurchaseView")
                .HasNoKey();
        }
    }
}
