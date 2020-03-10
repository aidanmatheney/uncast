namespace Uncast.Entities
{
    using System;

    public class UserPodcastEpisodeState
    {
        public Guid? UserId { get; set; }
        public Guid? EpisodeId { get; set; }
        public string? PlaybackStatus { get; set; }
        public int? ProgressMs { get; set; }
    }
}
