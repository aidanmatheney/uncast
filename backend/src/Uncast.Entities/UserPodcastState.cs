namespace Uncast.Entities
{
    using System;

    public class UserPodcastState
    {
        public Guid? UserId { get; set; }
        public Guid? PodcastId { get; set; }
        public bool IsSubscription { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsAutoDownload { get; set; }
        public decimal? PlaybackSpeed { get; set; }
    }
}
