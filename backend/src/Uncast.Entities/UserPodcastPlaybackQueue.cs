namespace Uncast.Entities
{
    using System;
    using System.Collections.Generic;

    public class UserPodcastPlaybackQueue
    {
        public Guid? UserId { get; set; }
        public IEnumerable<Guid>? EpisodeIds { get; set; }
    }
}
