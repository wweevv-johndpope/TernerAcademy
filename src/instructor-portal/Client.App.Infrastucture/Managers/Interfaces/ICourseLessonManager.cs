using Application.Common.Models;
using Application.InstructorPortal.CourseLessons.Commands.Delete;
using Application.InstructorPortal.CourseLessons.Commands.Update;
using Application.InstructorPortal.CourseLessons.Commands.UpdateOrdering;
using Application.InstructorPortal.CourseLessons.Commands.UploadLesson;
using Application.InstructorPortal.CourseLessons.Dtos;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ICourseLessonManager : IManager
    {
        Task<IResult<InstructorCourseLessonDto>> GetSingleAsync(int courseId, int lessonId);
        Task<IResult<List<InstructorCourseLessonDto>>> GetAllAsync(int courseId);
        Task<IResult> UploadAsync(UploadCourseLessonCommand request, Stream stream, string filename);
        Task<IResult> UpdateAsync(UpdateCourseLessonCommand request);
        Task<IResult> UpdateOrderingAsync(UpdateCourseLessonOrderingCommand request);
        Task<IResult> DeleteAsync(DeleteCourseLessonCommand request);
        Task<IResult<int>> GetProcessingCountAsync(int courseId);
    }
}