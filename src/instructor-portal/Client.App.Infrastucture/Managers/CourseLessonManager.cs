using Application.Common.Models;
using Application.InstructorPortal.CourseLessons.Commands.Delete;
using Application.InstructorPortal.CourseLessons.Commands.Update;
using Application.InstructorPortal.CourseLessons.Commands.UpdateOrdering;
using Application.InstructorPortal.CourseLessons.Commands.UploadLesson;
using Application.InstructorPortal.CourseLessons.Dtos;
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class CourseLessonManager : ManagerBase, ICourseLessonManager
    {
        private readonly ICourseLessonWebService _courseLessonWebService;

        public CourseLessonManager(IManagerToolkit managerToolkit, ICourseLessonWebService courseLessonWebService) : base(managerToolkit)
        {
            _courseLessonWebService = courseLessonWebService;
        }

        public async Task<IResult<InstructorCourseLessonDto>> GetSingleAsync(int courseId, int lessonId)
        {
            await PrepareForWebserviceCall();
            return await _courseLessonWebService.GetSingleAsync(courseId, lessonId, AccessToken);
        }

        public async Task<IResult<List<InstructorCourseLessonDto>>> GetAllAsync(int courseId)
        {
            await PrepareForWebserviceCall();
            return await _courseLessonWebService.GetAllAsync(courseId, AccessToken);
        }

        public async Task<IResult> UploadAsync(UploadCourseLessonCommand request, Stream stream, string filename)
        {
            await PrepareForWebserviceCall();
            return await _courseLessonWebService.UploadAsync(request, stream, filename, AccessToken);
        }

        public async Task<IResult> UpdateAsync(UpdateCourseLessonCommand request)
        {
            await PrepareForWebserviceCall();
            return await _courseLessonWebService.UpdateAsync(request, AccessToken);
        }

        public async Task<IResult> UpdateOrderingAsync(UpdateCourseLessonOrderingCommand request)
        {
            await PrepareForWebserviceCall();
            return await _courseLessonWebService.UpdateOrderingAsync(request, AccessToken);
        }

        public async Task<IResult> DeleteAsync(DeleteCourseLessonCommand request)
        {
            await PrepareForWebserviceCall();
            return await _courseLessonWebService.DeleteAsync(request, AccessToken);
        }
        public async Task<IResult<int>> GetProcessingCountAsync(int courseId)
        {
            await PrepareForWebserviceCall();
            return await _courseLessonWebService.GetProcessingCountAsync(courseId, AccessToken);
        }
    }
}
