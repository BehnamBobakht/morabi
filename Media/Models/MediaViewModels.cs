using System.Collections.Generic;

namespace Media.Models
{
    public class MediaCardViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int SessionCount { get; set; }
        public int CompletedCount { get; set; }
    }

    public class MediaSessionViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int SortOrder { get; set; }
        public int WatchedSeconds { get; set; }
        public int? DurationSeconds { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDemo { get; set; }
        public bool IsLocked { get; set; }
    }

    public class MediaWatchViewModel
    {
        public int OwnerId { get; set; }
        public string OwnerTitle { get; set; }
        public MediaSessionViewModel Current { get; set; }
        public IList<MediaSessionViewModel> Sessions { get; set; }
        public string StreamUrl { get; set; }
        public string ProgressUrl { get; set; }
        public bool OwnsCourse { get; set; }

    }
}
