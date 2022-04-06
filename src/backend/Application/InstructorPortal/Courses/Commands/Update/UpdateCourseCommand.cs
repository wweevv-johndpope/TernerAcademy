using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Courses.Commands.Update
{
    public class UpdateCourseCommand : IRequest<IResult>
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public double PriceInTFuel { get; set; }
        public IEnumerable<int> TopicIds { get; set; } = new List<int>();
        public int LanguageId { get; set; }

        public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public UpdateCourseCommandHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }


            public async Task<IResult> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
            {
                if (!AppConstants.CourseLevel.Any(x => x == request.Level)) return await Result.FailAsync("Invalid Course Level");

                request.TopicIds ??= new List<int>();
                if (!request.TopicIds.Any()) return await Result.FailAsync("Course Topic is required.");

                var course = await _dbContext.Courses.AsQueryable().FirstOrDefaultAsync(x => x.InstructorId == _context.UserId && x.Id == request.CourseId);

                if (course == null) return await Result.FailAsync("Course not found.");
                //if(course.ListingStatus != CourseListingStatus.Draft && course.Name != request.Name) return await Result.FailAsync("Course Name cannot be change anymore.");

                course.Name = request.Name;
                course.ShortDescription = request.ShortDescription;
                course.Description = request.Description;
                course.Level = request.Level;
                course.PriceInTFuel = request.PriceInTFuel;
                course.LanguageId = request.LanguageId;

                _dbContext.Courses.Update(course);
                await _dbContext.SaveChangesAsync();

                var currentTopics = await _dbContext.CourseTopics.AsQueryable().Where(x => x.CourseId == course.Id).ToListAsync();

                foreach (var currentTopic in currentTopics)
                {
                    if (!request.TopicIds.Any(x => x == currentTopic.Id))
                    {
                        _dbContext.CourseTopics.Remove(currentTopic);
                    }
                }

                await _dbContext.SaveChangesAsync();

                currentTopics = await _dbContext.CourseTopics.AsQueryable().Where(x => x.CourseId == course.Id).ToListAsync();

                foreach (var topicId in request.TopicIds)
                {
                    if (!currentTopics.Any(x => x.Id == topicId))
                    {
                        _dbContext.CourseTopics.Add(new CourseTopic() { CourseId = course.Id, TopicId = topicId });
                    }
                }

                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
