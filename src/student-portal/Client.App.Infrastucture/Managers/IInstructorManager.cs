using Application.Common.Models;
using Application.StudentPortal.Instructors.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IInstructorManager : IManager
    {
        Task<IResult<StudentInstructorDto>> GetDetailsAsync(int instructorId);
        Task<IResult<List<StudentInstructorCourseDto>>> GetCoursesAsync(int instructorId);
    }
}