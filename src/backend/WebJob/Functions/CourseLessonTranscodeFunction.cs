using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.InstructorPortal.CourseLessons.Commands.UpdateThetaMetadata;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebJob.Base;

namespace WebJob.Functions
{
    public class CourseLessonTranscodeFunction : FunctionBase
    {
        private readonly IThetaVideoService _videoService;
        private readonly IApplicationDbContext _dbContext;
        private readonly IAzureStorageBlobService _blobService;
        private readonly IAzureStorageQueueService _queueService;
        public CourseLessonTranscodeFunction(IMediator mediator, ICallContext context, IThetaVideoService videoService, IApplicationDbContext dbContext, IAzureStorageBlobService blobService, IAzureStorageQueueService queueService) : base(mediator, context)
        {
            _videoService = videoService;
            _dbContext = dbContext;
            _blobService = blobService;
            _queueService = queueService;
        }

        [FunctionName("CourseLesson_Transcode_Trigger1")]
        public async Task TranscodeBlobTrigger([QueueTrigger(QueueNames.Transcoder)] string lessonId,
              [DurableClient] IDurableOrchestrationClient starter, ILogger log, Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            log.LogInformation($"TranscodeBlobTrigger LessonId= '{lessonId}'.");
            await starter.StartNewAsync<string>("CourseLesson_Transcode_Orchestrator1", lessonId);
        }

        [FunctionName("CourseLesson_Transcode_Orchestrator1")]
        public async Task TranscodeOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            await context.CallActivityAsync<string>("CourseLesson_Transcode_Update1", context.GetInput<string>());
        }

        [FunctionName("CourseLesson_Transcode_Update1")]
        public void TranscodeUpdate([ActivityTrigger] string input, ILogger log)
        {
            var lessonId = int.Parse(input);
            var lesson = _dbContext.CourseLessons.AsQueryable().FirstOrDefault(x => x.Id == lessonId);

            if (lesson == null)
            {
                log.LogInformation($"[{lessonId}] Lesson not found.");
                return;
            }

            string videoId = lesson.ThetaVideoId;

            if (string.IsNullOrEmpty(lesson.ThetaVideoId))
            {
                var containerPath = _blobService.GetBlobContainerPath(BlobContainers.CourseLessons);
                var sourceUri = $"{containerPath}/{lesson.VideoPathUri}";

                var transcodeResult = _videoService.Transcode(sourceUri);
                if (transcodeResult == null || transcodeResult.Status == "error")
                {
                    if(transcodeResult != null && !transcodeResult.Message.Contains("Video limit reached"))
                    {
                        Thread.Sleep(30000);
                        _queueService.InsertMessage(QueueNames.Transcoder, input);
                    }

                    return;
                }

                videoId = transcodeResult.Body.Videos.First().Id;
                Mediator.Send(new UpdateCourseLessonThetaMetadataCommand() { LessonId = lessonId, ThetaVideoId = videoId }).Wait();
            }

            var transcodeResultProgress = _videoService.GetVideoProgress(videoId);

            if (transcodeResultProgress != null && transcodeResultProgress.Status == "success" && transcodeResultProgress.Body.Videos.Any())
            {
                var video = transcodeResultProgress.Body.Videos.First();

                if (video.State == "success" && video.Progress == 100)
                {
                    var finalLesson = _dbContext.CourseLessons.FirstOrDefaultAsync(x => x.Id == lessonId).Result;

                    if (finalLesson != null)
                    {
                        Mediator.Send(new UpdateCourseLessonThetaMetadataCommand()
                        {
                            LessonId = lessonId,
                            ThetaVideoId = videoId,
                            ThetaVideoDuration = long.Parse(video.Duration),
                            ThetaVideoPlayerUri = video.PlayerUri,
                            ThetaVideoPlaybackUri = video.PlaybackUri
                        }).Wait();
                    }

                    _blobService.DeleteAsync(BlobContainers.CourseLessons, lesson.VideoPathUri).Wait();
                }
                else
                {
                    Thread.Sleep(30000);
                    _queueService.InsertMessage(QueueNames.Transcoder, input);
                }
            }
            else
            {
                Thread.Sleep(30000);
                _queueService.InsertMessage(QueueNames.Transcoder, input);
            }
        }
    }
}
