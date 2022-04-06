using System.ComponentModel;

namespace Domain.Enums
{
    public enum CourseListingStatus
    {
        [Description("DRAFT")]
        Draft = 0,
        [Description("FOR APPROVAL")]
        ForApproval = 1,
        [Description("LISTED")]
        Approved = 2
    }
}
