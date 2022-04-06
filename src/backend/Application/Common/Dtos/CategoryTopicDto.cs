using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Dtos
{
    public class CategoryTopicDto : IMapFrom<CategoryTopic>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
