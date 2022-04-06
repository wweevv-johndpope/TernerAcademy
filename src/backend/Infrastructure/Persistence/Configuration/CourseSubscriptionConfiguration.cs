using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CourseSubscriptionConfiguration : IEntityTypeConfiguration<CourseSubscription>
    {
        public void Configure(EntityTypeBuilder<CourseSubscription> builder)
        {
            builder.ToTable("Subscriptions", "Course");

            builder.Property(t => t.Comment).HasMaxLength(512);
            builder.Property(t => t.BuyTx).HasMaxLength(128).IsRequired();
            builder.Property(t => t.CashoutPaymentTx).HasMaxLength(128);
            builder.Property(t => t.SendToBurnTx).HasMaxLength(128);
            builder.Property(t => t.SendToDevTx).HasMaxLength(128);
        }
    }
}
