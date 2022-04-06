using Application.Common.Models;
using Application.InstructorPortal.Courses.Commands.Create;
using Application.InstructorPortal.Courses.Commands.Publish;
using Application.InstructorPortal.Courses.Commands.Update;
using Application.InstructorPortal.Courses.Commands.UploadThumbnail;
using Application.InstructorPortal.Courses.Dtos;
using Application.InstructorPortal.Courses.Queries.GetDetails;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class CourseWebService : WebServiceBase, ICourseWebService
    {
        public CourseWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<List<InstructorCourseItemDto>>> GetAllAsync(string accessToken) => GetAsync<List<InstructorCourseItemDto>>(CourseEndpoints.GetAll, accessToken);
        public Task<IResult<List<InstructorListedCourseDto>>> GetAllListedAsync(string accessToken) => GetAsync<List<InstructorListedCourseDto>>(CourseEndpoints.GetAllListed, accessToken);
        public Task<IResult<CourseDetailsResponseDto>> GetDetailsAsync(int courseId, string accessToken) => GetAsync<CourseDetailsResponseDto>(string.Format(CourseEndpoints.GetDetails, courseId), accessToken);
        public Task<IResult<int>> CreateAsync(CreateCourseCommand request, string accessToken) => PostAsync<CreateCourseCommand,int>(CourseEndpoints.Create, request, accessToken);
        public Task<IResult> UpdateAsync(UpdateCourseCommand request, string accessToken) => PostAsync(string.Format(CourseEndpoints.Update, request.CourseId), request, accessToken);
        public Task<IResult> UploadThumbnailAsync(UploadCourseThumbnailCommand request, Stream fileStream, string filename, string accessToken) => PostFileAsync(string.Format(CourseEndpoints.UploadThumbnail, request.CourseId), request, fileStream, filename, accessToken);
        public Task<IResult> PublishAsync(PublishCourseCommand request, string accessToken) => PostAsync(string.Format(CourseEndpoints.Publish, request.CourseId), request, accessToken);
    }
}
