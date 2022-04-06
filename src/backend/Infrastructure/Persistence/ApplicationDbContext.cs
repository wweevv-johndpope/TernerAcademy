using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Views;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext(DbContextOptions options, IDomainEventService domainEventService, IDateTime dateTime) : base(options)
        {
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseLesson> CourseLessons { get; set; }
        public DbSet<CourseLanguage> CourseLanguages { get; set; }
        public DbSet<CourseTopic> CourseTopics { get; set; }
        public DbSet<CourseSubscription> CourseSubscriptions { get; set; }
        public DbSet<CourseLessonOrder> CourseLessonOrders { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTopic> CategoryTopics { get; set; }
        public DbSet<CategoryTopicViewItem> CategoryTopicViewItems { get; set; }

        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<InstructorPassword> InstructorPasswords { get; set; }
        public DbSet<InstructorCourseViewItem> InstructorCourseViewItems { get; set; }
        public DbSet<InstructorCommunity> InstructorCommunities { get; set; }
        public DbSet<InstructorCourseSubscriptionViewItem> InstructorCourseSubscriptionViewItems { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<StudentPassword> StudentPasswords { get; set; }
        public DbSet<StudentCategoryPreference> StudentCategoryPreferences { get; set; }
        public DbSet<StudentCourseViewItem> StudentCourseViewItems { get; set; }
        public DbSet<StudentEnrolledCourseViewItem> StudentEnrolledCourseViewItems { get; set; }
        public DbSet<StudentCourseSubscriptionViewItem> StudentCourseSubscriptionViewItems { get; set; }
        public DbSet<StudentCourseSubscriptionPurchaseViewItem> StudentCourseSubscriptionPurchaseViewItems { get; set; }
        public DbSet<StudentLesson> StudentLessons { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                var utcNow = _dateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = utcNow;
                        entry.Entity.UpdatedAt = utcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = utcNow;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            await DispatchEvents();
            return result;
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
                        .Select(x => x.Entity.DomainEvents)
                        .SelectMany(x => x)
                        .FirstOrDefault(domainEvent => !domainEvent.IsPublished);

                if (domainEventEntity == null) break;

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity);
            }
        }
    }
}