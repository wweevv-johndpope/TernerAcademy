using System;

namespace Domain.Views
{
    public class InstructorCourseSubscriptionViewItem
    {
        public int CourseId { get; set; }
        public string StudentName { get; set; }
        public string StudentProfilePictureUri { get; set; }
        public DateTime DateSubscribed { get; set; }
        public double Price { get; set; }
        public int ViewLessons { get; set; }
        public string CashoutPaymentTx { get; set; }
        public double? AmountCashout { get; set; }
        public string BurnTx { get; set; }
        public double? AmountBurn { get; set; }
    }
}
