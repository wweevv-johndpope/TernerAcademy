using Application.Common.Mappings;
using Domain.Views;

namespace Application.InstructorPortal.Courses.Dtos
{
    public class InstructorListedCourseDto : IMapFrom<InstructorCourseViewItem>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
