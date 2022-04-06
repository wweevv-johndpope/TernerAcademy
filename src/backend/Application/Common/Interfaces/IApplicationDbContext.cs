using Domain.Entities;
using Domain.Views;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Course> Courses { get; set; }
        DbSet<CourseLesson> CourseLessons { get; set; }
        DbSet<CourseLanguage> CourseLanguages { get; set; }
        DbSet<CourseTopic> CourseTopics { get; set; }
        DbSet<CourseSubscription> CourseSubscriptions { get; set; }
        DbSet<CourseLessonOrder> CourseLessonOrders { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<CategoryTopic> CategoryTopics { get; set; }
        DbSet<CategoryTopicViewItem> CategoryTopicViewItems { get; set; }

        DbSet<Instructor> Instructors { get; set; }
        DbSet<InstructorPassword> InstructorPasswords { get; set; }
        DbSet<InstructorCourseViewItem> InstructorCourseViewItems { get; set; }
        DbSet<InstructorCommunity> InstructorCommunities { get; set; }
        DbSet<InstructorCourseSubscriptionViewItem> InstructorCourseSubscriptionViewItems { get; set; }

        DbSet<Student> Students { get; set; }
        DbSet<StudentPassword> StudentPasswords { get; set; }
        DbSet<StudentCategoryPreference> StudentCategoryPreferences { get; set; }
        DbSet<StudentCourseViewItem> StudentCourseViewItems { get; set; }
        DbSet<StudentEnrolledCourseViewItem> StudentEnrolledCourseViewItems { get; set; }
        DbSet<StudentCourseSubscriptionViewItem> StudentCourseSubscriptionViewItems { get; set; }
        DbSet<StudentCourseSubscriptionPurchaseViewItem> StudentCourseSubscriptionPurchaseViewItems { get; set; }

        DbSet<StudentLesson> StudentLessons { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        int SaveChanges();
    }
}