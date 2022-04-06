using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Courses.EventHandlers
{
    public class NewCourseReviewEventHandler : INotificationHandler<DomainEventNotification<NewCourseReviewEvent>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IDateTime _dateTime;

        public NewCourseReviewEventHandler(IApplicationDbContext dbContext, IDateTime dateTime)
        {
            _dbContext = dbContext;
            _dateTime = dateTime;
        }

        public async Task Handle(DomainEventNotification<NewCourseReviewEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            var course = await _dbContext.Courses.AsQueryable().FirstOrDefaultAsync(x => x.Id == domainEvent.CourseId);
            var subscriptions = await _dbContext.CourseSubscriptions.AsQueryable().Where(x => x.CourseId == domainEvent.CourseId && x.Rating.HasValue).ToListAsync();

            if (subscriptions.Any())
            {
                course.AverageRating = Convert.ToDouble(subscriptions.Sum(x => x.Rating.Value)) / subscriptions.Count;
                course.RatingCount = subscriptions.Count;
            }
            else
            {
                course.AverageRating = 0;
                course.RatingCount = 0;
            }

            _dbContext.Courses.Update(course);
            await _dbContext.SaveChangesAsync();
        }
    }
}
