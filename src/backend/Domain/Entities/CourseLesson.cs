namespace Domain.Entities
{
    public class CourseLesson
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsPreviewable { get; set; }

        public string VideoPathUri { get; set; }

        public string ThetaVideoId { get; set; }
        public string ThetaVideoPlayerUri { get; set; }
        public string ThetaVideoPlaybackUri { get; set; }
        public long ThetaVideoDuration { get; set; }

        //public int? ChildLessonId { get; set; }
    }
}
