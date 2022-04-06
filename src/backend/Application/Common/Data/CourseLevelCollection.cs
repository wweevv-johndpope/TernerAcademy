using System.Collections.Generic;

namespace Application.Common.Models
{
    public class CourseLevelCollection : List<string>
    {
        public CourseLevelCollection()
        {
            Clear();
            Add("Beginner");
            Add("Intermediate");
            Add("Advanced");
        }
    }
}
