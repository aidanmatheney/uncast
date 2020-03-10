namespace Uncast.Entities
{
    using System;

    public class UserAppState
    {
        public Guid? UserId { get; set; }
        public string? ColorScheme { get; set; }
        public string? DefaultPlaybackStyle { get; set; }
        public decimal? DefaultPlaybackSpeed { get; set; }
    }
}
