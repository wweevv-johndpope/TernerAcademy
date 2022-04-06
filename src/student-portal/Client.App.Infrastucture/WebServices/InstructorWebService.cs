using Application.Common.Models;
using Application.StudentPortal.Instructors.Dtos;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class InstructorWebService : WebServiceBase, IInstructorWebService
    {
        public InstructorWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<StudentInstructorDto>> GetDetailsAsync(int instructorId, string accessToken) => GetAsync<StudentInstructorDto>(string.Format(InstructorEndpoints.GetDetails, instructorId), accessToken);
        public Task<IResult<List<StudentInstructorCourseDto>>> GetCoursesAsync(int instructorId, string accessToken) => GetAsync<List<StudentInstructorCourseDto>>(string.Format(InstructorEndpoints.GetCourses, instructorId), accessToken);
    }
}
