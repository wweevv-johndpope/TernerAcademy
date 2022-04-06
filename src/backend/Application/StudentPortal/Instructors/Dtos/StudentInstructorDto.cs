using Application.Common.Mappings;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.StudentPortal.Instructors.Dtos
{
    public class StudentInstructorDto : IMapFrom<Instructor>
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUri { get; set; }

        public List<StudentInstructorCommunityDto> Commmunities { get; set; }
    }
}
