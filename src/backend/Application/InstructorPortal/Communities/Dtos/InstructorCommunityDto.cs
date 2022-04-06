using Application.Common.Mappings;
using Domain.Entities;

namespace Application.InstructorPortal.Communities.Dtos
{
    public class InstructorCommunityDto : IMapFrom<InstructorCommunity>
    {
        public int Id { get; set; }
        public string Platform { get; set; }
        public string HandleName { get; set; }
    }
}
