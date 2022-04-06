using Application.Common.Models;
using Application.InstructorPortal.CourseLessons.Commands.Delete;
using Application.InstructorPortal.CourseLessons.Commands.Update;
using Application.InstructorPortal.CourseLessons.Commands.UpdateOrdering;
using Application.InstructorPortal.CourseLessons.Commands.UploadLesson;
using Application.InstructorPortal.CourseLessons.Dtos;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class CourseLessonWebService : WebServiceBase, ICourseLessonWebService
    {
        public CourseLessonWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<InstructorCourseLessonDto>> GetSingleAsync(int courseId, int lessonId, string accessToken) => GetAsync<InstructorCourseLessonDto>(string.Format(CourseLessonEndpoints.Get, courseId, lessonId), accessToken);
        public Task<IResult<List<InstructorCourseLessonDto>>> GetAllAsync(int courseId, string accessToken) => GetAsync<List<InstructorCourseLessonDto>>(string.Format(CourseLessonEndpoints.GetAll, courseId), accessToken);
        public Task<IResult> UploadAsync(UploadCourseLessonCommand request, Stream fileStream, string filename, string accessToken) => PostFileAsync(CourseLessonEndpoints.Upload, request, fileStream, filename, accessToken);
        public Task<IResult> UpdateAsync(UpdateCourseLessonCommand request, string accessToken) => PostAsync(string.Format(CourseLessonEndpoints.Update, request.LessonId), request, accessToken);
        public Task<IResult> UpdateOrderingAsync(UpdateCourseLessonOrderingCommand request, string accessToken) => PostAsync(string.Format(CourseLessonEndpoints.UpdateOrdering, request.CourseId), request, accessToken);
        public Task<IResult> DeleteAsync(DeleteCourseLessonCommand request, string accessToken) => PostAsync(string.Format(CourseLessonEndpoints.Delete, request.CourseId, request.LessonId), request, accessToken);
        public Task<IResult<int>> GetProcessingCountAsync(int courseId, string accessToken) => GetAsync<int>(string.Format(CourseLessonEndpoints.GetProcessingCount, courseId), accessToken);

    }
}
