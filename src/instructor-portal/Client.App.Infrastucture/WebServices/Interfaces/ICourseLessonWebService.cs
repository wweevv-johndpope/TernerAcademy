using Application.Common.Models;
using Application.InstructorPortal.CourseLessons.Commands.Delete;
using Application.InstructorPortal.CourseLessons.Commands.Update;
using Application.InstructorPortal.CourseLessons.Commands.UpdateOrdering;
using Application.InstructorPortal.CourseLessons.Commands.UploadLesson;
using Application.InstructorPortal.CourseLessons.Dtos;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface ICourseLessonWebService : IWebService
    {
        Task<IResult<InstructorCourseLessonDto>> GetSingleAsync(int courseId, int lessonId, string accessToken);
        Task<IResult<List<InstructorCourseLessonDto>>> GetAllAsync(int courseId, string accessToken);
        Task<IResult> UploadAsync(UploadCourseLessonCommand request, Stream fileStream, string filename, string accessToken);
        Task<IResult> UpdateAsync(UpdateCourseLessonCommand request, string accessToken);
        Task<IResult> UpdateOrderingAsync(UpdateCourseLessonOrderingCommand request, string accessToken);
        Task<IResult> DeleteAsync(DeleteCourseLessonCommand request, string accessToken);
        Task<IResult<int>> GetProcessingCountAsync(int courseId, string accessToken);
    }
}