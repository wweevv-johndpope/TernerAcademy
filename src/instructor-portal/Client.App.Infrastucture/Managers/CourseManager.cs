using Application.Common.Models;
using Application.InstructorPortal.Courses.Commands.Create;
using Application.InstructorPortal.Courses.Commands.Publish;
using Application.InstructorPortal.Courses.Commands.Update;
using Application.InstructorPortal.Courses.Commands.UploadThumbnail;
using Application.InstructorPortal.Courses.Dtos;
using Application.InstructorPortal.Courses.Queries.GetDetails;
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class CourseManager : ManagerBase, ICourseManager
    {
        private readonly ICourseWebService _courseWebService;
        public CourseManager(IManagerToolkit managerToolkit, ICourseWebService courseWebService) : base(managerToolkit)
        {
            _courseWebService = courseWebService;
        }

        public async Task<IResult<List<InstructorCourseItemDto>>> GetAllAsync()
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.GetAllAsync(AccessToken);
        }

        public async Task<IResult<List<InstructorListedCourseDto>>> GetAllListedAsync()
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.GetAllListedAsync(AccessToken);
        }

        public async Task<IResult<CourseDetailsResponseDto>> GetDetailsAsync(int courseId)
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.GetDetailsAsync(courseId, AccessToken);
        }

        public async Task<IResult<int>> CreateAsync(CreateCourseCommand request)
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.CreateAsync(request, AccessToken);
        }

        public async Task<IResult> UpdateAsync(UpdateCourseCommand request)
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.UpdateAsync(request, AccessToken);
        }

        public async Task<IResult> UploadThumbnailAsync(UploadCourseThumbnailCommand request, Stream stream, string filename)
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.UploadThumbnailAsync(request, stream, filename, AccessToken);
        }

        public async Task<IResult> PublishAsync(PublishCourseCommand request)
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.PublishAsync(request, AccessToken);
        }
    }
}
