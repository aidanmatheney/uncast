namespace Uncast.Entities
{
    using System;

    public class AppFile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Path { get; set; }
        public string? OriginalName { get; set; }
    }
}