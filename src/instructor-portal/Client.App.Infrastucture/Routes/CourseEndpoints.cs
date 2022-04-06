namespace Client.App.Infrastructure.Routes
{
    public static class CourseEndpoints
    {
        public const string GetAll = "api/instructor/course";
        public const string GetAllListed = "api/instructor/course/listed";
        public const string GetDetails = "api/instructor/course/{0}";
        public const string Create = "api/instructor/course";
        public const string Update = "api/instructor/course/{0}";
        public const string UploadThumbnail = "api/instructor/course/{0}/thumbnail";
        public const string Publish = "api/instructor/course/{0}/publish";
    }
}
