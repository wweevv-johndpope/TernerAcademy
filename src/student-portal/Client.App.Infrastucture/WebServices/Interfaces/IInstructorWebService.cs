using Application.Common.Models;
using Application.StudentPortal.Instructors.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IInstructorWebService : IWebService
    {
        Task<IResult<StudentInstructorDto>> GetDetailsAsync(int instructorId, string accessToken);
        Task<IResult<List<StudentInstructorCourseDto>>> GetCoursesAsync(int instructorId, string accessToken);
    }
}