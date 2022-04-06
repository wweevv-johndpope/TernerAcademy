namespace Client.App.Infrastructure.Routes
{
    public static class CourseLessonEndpoints
    {
        public const string Get = "api/instructor/course/{0}/lesson/{1}";
        public const string GetAll = "api/instructor/course/{0}/lesson";
        public const string Upload = "api/instructor/course/lesson";
        public const string Update = "api/instructor/course/lesson/{0}";
        public const string UpdateOrdering = "api/instructor/course/{0}/lesson/order";
        public const string Delete = "api/instructor/course/{0}/lesson/{1}/delete";
        public const string GetProcessingCount = "api/instructor/course/{0}/lesson/processing";
    }
}
