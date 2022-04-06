using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CourseLessons.Commands.UpdateThetaMetadata
{
    public class UpdateCourseLessonThetaMetadataCommand : IRequest<IResult>
    {
        public int LessonId { get; set; }
        public string ThetaVideoId { get; set; }
        public string ThetaVideoPlayerUri { get; set; }
        public string ThetaVideoPlaybackUri { get; set; }
        public long ThetaVideoDuration { get; set; }

        public class UpdateCourseLessonThetaMetadataCommandHandler : IRequestHandler<UpdateCourseLessonThetaMetadataCommand, IResult>
        {
            private readonly IApplicationDbContext _dbContext;

            public UpdateCourseLessonThetaMetadataCommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(UpdateCourseLessonThetaMetadataCommand request, CancellationToken cancellationToken)
            {
                var lesson = await _dbContext.CourseLessons.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.LessonId);

                if (lesson == null) return await Result.FailAsync("Course Lesson not found.");

                lesson.ThetaVideoId = request.ThetaVideoId;

                if (!string.IsNullOrEmpty(request.ThetaVideoPlayerUri))
                {
                    lesson.ThetaVideoPlayerUri = request.ThetaVideoPlayerUri;
                    lesson.ThetaVideoPlaybackUri = request.ThetaVideoPlaybackUri;
                    lesson.ThetaVideoDuration = request.ThetaVideoDuration;
                }

                _dbContext.CourseLessons.Update(lesson);
                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
