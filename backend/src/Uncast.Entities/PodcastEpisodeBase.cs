namespace Uncast.Entities
{
    using System;

    public abstract class PodcastEpisodeBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? PodcastId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}