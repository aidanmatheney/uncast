namespace Uncast.Entities
{
    using System;

    public abstract class PodcastBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public Guid? ThumbnailFileId { get; set; }
    }
}