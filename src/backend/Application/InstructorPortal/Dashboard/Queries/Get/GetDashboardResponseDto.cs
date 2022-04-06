namespace Application.InstructorPortal.Dashboard.Queries.Get
{
    public class GetDashboardResponseDto
    {
        public int RegisteredStudentCount { get; set; }
        public int TotalListedCourseCount { get; set; }
        public double TotalAmountPayouts { get; set; }
        public double TotalAmountBurned { get; set; }

        public int InstructorUniqueStudentCount { get; set; }
        public int InstructorCourseCount { get; set; }
        public int InstructorCourseLessonCount { get; set; }
        public int InstructorCoursePurchaseCount { get; set; }

        public double InstructorTotalEarnings { get; set; }
        public double InstructorAverageRating { get; set; }
    }
}
