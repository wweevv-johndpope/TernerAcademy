using System;

namespace Application.InstructorPortal.CourseSubscriptions.Dtos
{
    public class InstructorCourseSubscriptionDto
    {
        public string StudentName { get; set; }
        public string StudentProfilePictureUri { get; set; }
        public double Price { get; set; }
        public double Progress { get; set; }
        public DateTime DateSubscribed { get; set; }
        public string CashoutPaymentTx { get; set; }
        public double? AmountCashout { get; set; }
        public string BurnTx { get; set; }
        public double? AmountBurn { get; set; }
    }
}
