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

namespace Client.App.Infrastructure.WebServices
{
    public interface ICourseWebService : IWebService
    {
        Task<IResult<List<InstructorCourseItemDto>>> GetAllAsync(string accessToken);
        Task<IResult<List<InstructorListedCourseDto>>> GetAllListedAsync(string accessToken);
        Task<IResult<CourseDetailsResponseDto>> GetDetailsAsync(int courseId, string accessToken);
        Task<IResult<int>> CreateAsync(CreateCourseCommand request, string accessToken);
        Task<IResult> UpdateAsync(UpdateCourseCommand request, string accessToken);
        Task<IResult> UploadThumbnailAsync(UploadCourseThumbnailCommand request, Stream fileStream, string filename, string accessToken);
        Task<IResult> PublishAsync(PublishCourseCommand request, string accessToken);
    }
}