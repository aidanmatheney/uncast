namespace Uncast.Entities
{
    using System;

    public abstract class CustomPodcastBase : PodcastBase
    {
        public Guid? UserId { get; set; }
    }
}