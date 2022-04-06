using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Courses.Commands.Create
{
    public class CreateCourseCommand : IRequest<Result<int>>
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public double PriceInTFuel { get; set; }
        public IEnumerable<int> TopicIds { get; set; } = new List<int>();
        public int LanguageId { get; set; }

        public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Result<int>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public CreateCourseCommandHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<Result<int>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
            {
                if (!AppConstants.CourseLevel.Any(x => x == request.Level)) return await Result<int>.FailAsync("Invalid Course Level");

                request.TopicIds ??= new List<int>();
                if (!request.TopicIds.Any()) return await Result<int>.FailAsync("Course Topic is required.");

                var isExists = await _dbContext.Courses.AsQueryable().AnyAsync(x => x.Name == request.Name);

                if (isExists) return await Result<int>.FailAsync("Course Name is already taken.");

                var course = new Course()
                {
                    Name = request.Name,
                    InstructorId = _context.UserId,
                    ShortDescription = request.ShortDescription,
                    Description = request.Description,
                    Level = request.Level,
                    PriceInTFuel = request.PriceInTFuel,
                    LanguageId = request.LanguageId,
                };

                _dbContext.Courses.Add(course);
                await _dbContext.SaveChangesAsync();

                foreach (var topicId in request.TopicIds)
                {
                    _dbContext.CourseTopics.Add(new CourseTopic() { CourseId = course.Id, TopicId = topicId });
                }

                await _dbContext.SaveChangesAsync();

                return await Result<int>.SuccessAsync(course.Id);
            }
        }
    }
}
