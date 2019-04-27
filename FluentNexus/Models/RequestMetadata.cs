using System;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>Metadata about rate limits and the last request.</summary>
    public class RequestMetadata
    {
        /// <summary>The number of API requests allowed per day.</summary>
        public int DailyLimit { get; set; }

        /// <summary>The number of API requests left for the current day before rate limits are enforced.</summary>
        public int DailyRemaining { get; set; }

        /// <summary>When usage towards the <see cref="DailyLimit"/> will reset to zero.</summary>
        public DateTimeOffset DailyReset { get; set; }

        /// <summary>The number of API requests allowed per hour.</summary>
        public int HourlyLimit { get; set; }

        /// <summary>The number of API requests left for the current hour before rate limits are enforced.</summary>
        public int HourlyRemaining { get; set; }

        /// <summary>When usage towards the <see cref="HourlyLimit"/> will reset to zero.</summary>
        public DateTimeOffset HourlyReset { get; set; }

        /// <summary>The number of seconds the most recent request took to process on the server.</summary>
        public float LastRequestRuntime { get; set; }
    }
}
