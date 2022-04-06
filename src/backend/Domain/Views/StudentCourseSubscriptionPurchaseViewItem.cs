using System;

namespace Domain.Views
{
    public class StudentCourseSubscriptionPurchaseViewItem
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string InstructorProfilePictureFilename { get; set; }
        public int SubscriptionId { get; set; }
        public DateTime DateBought { get; set; }
        public double BuyAmount { get; set; }
        public string BuyTx { get; set; }

    }
}
