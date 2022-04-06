using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Common.Dtos
{
    public class CategoryDto : IMapFrom<Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
