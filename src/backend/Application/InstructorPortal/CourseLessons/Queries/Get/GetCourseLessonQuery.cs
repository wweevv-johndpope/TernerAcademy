using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.CourseLessons.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CourseLessons.Queries.Get
{
    public class GetCourseLessonQuery : IRequest<Result<InstructorCourseLessonDto>>
    {
        public int CourseId { get; set; }
        public int LessonId { get; set; }

        public class GetCourseLessonQueryHandler : IRequestHandler<GetCourseLessonQuery, Result<InstructorCourseLessonDto>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public GetCourseLessonQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Result<InstructorCourseLessonDto>> Handle(GetCourseLessonQuery request, CancellationToken cancellationToken)
            {
                var isCourseExists = await _dbContext.Courses.AsQueryable().AnyAsync(x => x.Id == request.CourseId && x.InstructorId == _context.UserId);
                if (!isCourseExists) return await Result<InstructorCourseLessonDto>.FailAsync("Course not found.");

                var lesson = _dbContext.CourseLessons.AsQueryable().FirstOrDefault(x => x.CourseId == request.CourseId && x.Id == request.LessonId);
                if (lesson == null) return await Result<InstructorCourseLessonDto>.FailAsync("Course Lesson not found.");

                var mappedLesson = _mapper.Map<InstructorCourseLessonDto>(lesson);
                return await Result<InstructorCourseLessonDto>.SuccessAsync(mappedLesson);
            }
        }
    }
}
