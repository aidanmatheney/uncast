namespace Uncast.Entities
{
    using System;

    public class WebApiLogEntry
    {
        public int Id { get; set; }

        public DateTimeOffset TimeWritten { get; set; }
        public string? ServerName { get; set; }

        public string? Category { get; set; }
        public string? Scope { get; set; }
        public string? LogLevel { get; set; }
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
    }
}