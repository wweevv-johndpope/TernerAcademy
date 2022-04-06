using Application.Common.Models;
using Application.InstructorPortal.Courses.Commands.Create;
using Application.InstructorPortal.Courses.Commands.Publish;
using Application.InstructorPortal.Courses.Commands.Update;
using Application.InstructorPortal.Courses.Commands.UploadThumbnail;
using Application.InstructorPortal.Courses.Dtos;
using Application.InstructorPortal.Courses.Queries.GetDetails;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ICourseManager : IManager
    {
        Task<IResult<List<InstructorCourseItemDto>>> GetAllAsync();
        Task<IResult<List<InstructorListedCourseDto>>> GetAllListedAsync();
        Task<IResult<CourseDetailsResponseDto>> GetDetailsAsync(int courseId);
        Task<IResult<int>> CreateAsync(CreateCourseCommand request);
        Task<IResult> UpdateAsync(UpdateCourseCommand request);
        Task<IResult> UploadThumbnailAsync(UploadCourseThumbnailCommand request, Stream stream, string filename);
        Task<IResult> PublishAsync(PublishCourseCommand request);
    }
}