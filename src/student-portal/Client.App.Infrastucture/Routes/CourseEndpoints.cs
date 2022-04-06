namespace Client.App.Infrastructure.Routes
{
    public static class CourseEndpoints
    {
        public const string Search = "api/student/course/search";
        public const string GetPreview = "api/student/course/{0}/preview";
        public const string GetDetails = "api/student/course/{0}/details";
        public const string Buy = "api/student/course/{0}/buy";
        public const string GetEnrolledCourses = "api/student/course/enrolled";
        public const string WatchLesson = "api/student/course/{0}/watch";
        public const string WriteCourseReview = "api/student/course/{0}/review";
    }
}
