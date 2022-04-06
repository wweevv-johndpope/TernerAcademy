using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Routes
{
    public static class InstructorEndpoints
    {
        public const string GetDetails = "api/student/instructor/{0}/details";
        public const string GetCourses = "api/student/instructor/{0}/courses";
    }
}
