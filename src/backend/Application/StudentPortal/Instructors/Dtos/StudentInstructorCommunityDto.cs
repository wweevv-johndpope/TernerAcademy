using Application.Common.Mappings;
using Domain.Entities;

namespace Application.StudentPortal.Instructors.Dtos
{
    public class StudentInstructorCommunityDto : IMapFrom<InstructorCommunity>
    {
        public string Platform { get; set; }
        public string HandleName { get; set; }
    }
}
