using System.Collections.Generic;

namespace Application.Common.Constants
{
    public static class AppConstants
    {
        public const int MinimumPasswordLength = 8;
        public const string AllowableProfilePhotoFormat = ".jpg, .png, .jpeg";
        public const string AllowableImageFormat = ".jpg, .png, .jpeg";
        public const string AllowableVideoFormat = ".mp4, .mkv";
        public static readonly List<string> CourseLevel = new List<string>() { "Beginner", "Intermediate", "Advanced" };
        public static readonly List<string> CommunityPlatforms = new List<string>() { "Instagram", "Telegram", "Twitter" };
    }
}
