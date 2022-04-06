using Application.Common.Models;
using Application.StudentPortal.Instructors.Dtos;
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class InstructorManager : ManagerBase, IInstructorManager
    {
        private readonly IInstructorWebService _instructorWebService;
        public InstructorManager(IManagerToolkit managerToolkit, IInstructorWebService instructorWebService) : base(managerToolkit)
        {
            _instructorWebService = instructorWebService;
        }

        public async Task<IResult<StudentInstructorDto>> GetDetailsAsync(int instructorId)
        {
            await PrepareForWebserviceCall();
            return await _instructorWebService.GetDetailsAsync(instructorId, AccessToken);
        }

        public async Task<IResult<List<StudentInstructorCourseDto>>> GetCoursesAsync(int instructorId)
        {
            await PrepareForWebserviceCall();
            return await _instructorWebService.GetCoursesAsync(instructorId, AccessToken);
        }
    }
}
