using Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Views
{
    public class CategoryTopicViewConfiguration : IEntityTypeConfiguration<CategoryTopicViewItem>
    {
        public void Configure(EntityTypeBuilder<CategoryTopicViewItem> builder)
        {
            builder.ToView("CategoryTopicView")
                .HasNoKey();

            /*
CREATE VIEW CategoryTopicView AS
SELECT T1.Id as CategoryId, T1.Name as Category, T2.Id as TopicId, T2.Name as Topic FROM [Category].Categories T1
JOIN [Category].Topics T2 ON T1.Id = T2.CategoryId
             */
        }
    }
}
