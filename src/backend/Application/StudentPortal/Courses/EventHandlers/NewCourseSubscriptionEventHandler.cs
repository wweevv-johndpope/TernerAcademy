using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Courses.EventHandlers
{
    public class NewCourseSubscriptionEventHandler : INotificationHandler<DomainEventNotification<NewCourseSubscriptionEvent>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IDateTime _dateTime;

        public NewCourseSubscriptionEventHandler(IApplicationDbContext dbContext, IDateTime dateTime)
        {
            _dbContext = dbContext;
            _dateTime = dateTime;
        }

        public async Task Handle(DomainEventNotification<NewCourseSubscriptionEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            var course = await _dbContext.Courses.AsQueryable().FirstOrDefaultAsync(x => x.Id == domainEvent.CourseId);
            var subscriptions = await _dbContext.CourseSubscriptions.AsQueryable().CountAsync(x => x.CourseId == domainEvent.CourseId);

            course.EnrolledCount = subscriptions;

            _dbContext.Courses.Update(course);
            await _dbContext.SaveChangesAsync();
        }
    }
}
