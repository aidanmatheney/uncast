namespace Uncast.Entities
{
    using System;

    public class WebAppLogEntry
    {
        public int Id { get; set; }

        public DateTimeOffset TimeWritten { get; set; }
    }
}