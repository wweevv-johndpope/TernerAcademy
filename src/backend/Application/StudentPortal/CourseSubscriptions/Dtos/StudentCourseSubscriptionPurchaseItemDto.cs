using Application.Common.Mappings;
using Domain.Views;
using System;

namespace Application.StudentPortal.CourseSubscriptions.Dtos
{
    public class StudentCourseSubscriptionPurchaseItemDto : IMapFrom<StudentCourseSubscriptionPurchaseViewItem>
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string InstructorProfilePictureUri { get; set; }
        public int SubscriptionId { get; set; }
        public DateTime DateBought { get; set; }
        public double BuyAmount { get; set; }
        public string BuyTx { get; set; }
    }
}
