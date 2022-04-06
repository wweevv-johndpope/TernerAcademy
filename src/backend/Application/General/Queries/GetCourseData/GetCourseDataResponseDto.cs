using Application.Common.Dtos;
using Domain.Views;
using System.Collections.Generic;

namespace Application.General.Queries.GetCourseData
{
    public class GetCourseDataResponseDto
    {
        public List<CategoryTopicViewItem> CategoryData { get; set; }
        public List<CourseLanguageDto> LanguageData { get; set; }
    }
}
