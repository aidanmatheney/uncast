namespace Uncast.Entities
{
    using System;

    public class CustomFilePodcastEpisode : CustomPodcastEpisodeBase
    {
        public Guid? FileId { get; set; }
    }
}