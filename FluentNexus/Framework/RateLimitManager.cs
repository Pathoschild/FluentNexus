using System;
using System.Collections.Generic;
using System.Linq;
using Pathoschild.Http.Client;

namespace Pathoschild.FluentNexus.Framework
{
    /// <summary>Provides access to the Nexus Mods rate limits with utility helpers.</summary>
    internal class RateLimitManager : IRateLimitManager
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The number of API requests allowed per day.</summary>
        public int DailyLimit { get; private set; }

        /// <summary>The number of API requests left for the current day before rate limits are enforced.</summary>
        public int DailyRemaining { get; private set; }

        /// <summary>When usage towards the <see cref="IRateLimitManager.DailyLimit"/> will reset to zero.</summary>
        public DateTimeOffset DailyReset { get; private set; }

        /// <summary>The number of API requests allowed per hour.</summary>
        public int HourlyLimit { get; private set; }

        /// <summary>The number of API requests left for the current hour before rate limits are enforced.</summary>
        public int HourlyRemaining { get; private set; }

        /// <summary>When usage towards the <see cref="IRateLimitManager.HourlyLimit"/> will reset to zero.</summary>
        public DateTimeOffset HourlyReset { get; private set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Update the rate limit info.</summary>
        /// <param name="response">The response from which to read rate limit data.</param>
        public void UpdateFrom(IResponse response)
        {
            T GetHeaderValue<T>(string name, Func<string, T> parse)
            {
                if (response.Message.Headers.TryGetValues(name, out IEnumerable<string> values))
                {
                    string value = values.FirstOrDefault();
                    if (value != null)
                    {
                        try
                        {
                            return parse(value);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException($"The response includes unexpected {name} value: '{value}' can't be converted to {typeof(T).FullName}.", ex);
                        }
                    }
                }

                throw new InvalidOperationException($"The response doesn't include the expected {name} header.");
            }

            this.DailyLimit = GetHeaderValue("x-rl-daily-limit", int.Parse);
            this.DailyRemaining = GetHeaderValue("x-rl-daily-remaining", int.Parse);
            this.DailyReset = GetHeaderValue("x-rl-daily-reset", DateTimeOffset.Parse);
            this.HourlyLimit = GetHeaderValue("x-rl-hourly-limit", int.Parse);
            this.HourlyRemaining = GetHeaderValue("x-rl-hourly-remaining", int.Parse);
            this.HourlyReset = GetHeaderValue("x-rl-hourly-reset", DateTimeOffset.Parse);
        }

        /// <summary>Get whether the cached data is out of date.</summary>
        public bool IsOutdated()
        {
            return this.HourlyReset < DateTimeOffset.UtcNow || this.DailyReset < DateTimeOffset.UtcNow;
        }

        /// <summary>Get whether no requests can be submitted because <see cref="IRateLimitManager.DailyRemaining"/> and <see cref="IRateLimitManager.HourlyRemaining"/> are both zero.</summary>
        public bool IsBlocked()
        {
            return this.DailyRemaining <= 0 && this.HourlyRemaining <= 0;
        }

        /// <summary>Get the time until the next reset if <see cref="IRateLimitManager.IsBlocked"/> returns true, else <see cref="TimeSpan.Zero"/>.</summary>
        public TimeSpan GetTimeUntilRenewal()
        {
            if (!this.IsBlocked())
                return TimeSpan.Zero;

            DateTimeOffset reset = this.HourlyReset <= this.DailyReset ? this.HourlyReset : this.DailyReset;
            return reset - DateTimeOffset.UtcNow;
        }
    }
}
