using System;

namespace Pathoschild.FluentNexus
{
    /// <summary>Provides access to the Nexus Mods rate limits with utility helpers.</summary>
    public interface IRateLimitManager
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The number of API requests allowed per day.</summary>
        int DailyLimit { get; }

        /// <summary>The number of API requests left for the current day before rate limits are enforced.</summary>
        int DailyRemaining { get; }

        /// <summary>When usage towards the <see cref="DailyLimit"/> will reset to zero.</summary>
        DateTimeOffset DailyReset { get; }

        /// <summary>The number of API requests allowed per hour.</summary>
        int HourlyLimit { get; }

        /// <summary>The number of API requests left for the current hour before rate limits are enforced.</summary>
        int HourlyRemaining { get; }

        /// <summary>When usage towards the <see cref="HourlyLimit"/> will reset to zero.</summary>
        DateTimeOffset HourlyReset { get; }


        /*********
        ** Public methods
        *********/
        /// <summary>Get whether no requests can be submitted because <see cref="DailyRemaining"/> and <see cref="HourlyRemaining"/> are both zero.</summary>
        bool IsBlocked();

        /// <summary>Get the time until the next reset if <see cref="IsBlocked"/> returns true, else <see cref="TimeSpan.Zero"/>.</summary>
        TimeSpan GetTimeUntilRenewal();
    }
}
