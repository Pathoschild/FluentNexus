using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pathoschild.FluentNexus.Endpoints;
using Pathoschild.FluentNexus.Framework;
using Pathoschild.FluentNexus.Models;
using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;

namespace Pathoschild.FluentNexus
{
    /// <summary>A client for the Nexus Mods web API.</summary>
    public class NexusClient
    {
        /*********
        ** Fields
        *********/
        /// <summary>Metadata about the most recent request, if applicable.</summary>
        private RequestMetadata LastRequestMetadata;


        /*********
        ** Accessors
        *********/
        /// <summary>The underlying HTTP client.</summary>
        public IClient HttpClient { get; }

        /// <summary>The color schemes endpoints.</summary>
        public NexusColorSchemesClient ColorSchemes { get; }

        /// <summary>The games endpoints.</summary>
        public NexusGamesClient Games { get; }

        /// <summary>The mods endpoints.</summary>
        public NexusModsClient Mods { get; }

        /// <summary>The mod files endpoints.</summary>
        public NexusModFilesClient ModFiles { get; }

        /// <summary>The users endpoints.</summary>
        public NexusUsersClient Users { get; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="apiKey">The Nexus API key with which to authenticate.</param>
        /// <param name="baseUrl">The base URL for the API.</param>
        public NexusClient(string apiKey, string baseUrl = "https://api.nexusmods.com")
        {
            // init client
            this.HttpClient = new FluentClient(baseUrl)
                .SetUserAgent(this.GetDefaultUserAgent())
                .AddDefault(p => p.WithHeader("apiKey", apiKey));

            // add filters
            this.HttpClient.Filters.Remove<DefaultErrorFilter>();
            this.HttpClient.Filters.Add(new NexusErrorFilter());
            this.HttpClient.Filters.Add(new ResponseCallbackFilter(this.OnResponseReceived));

            // init endpoints
            this.ColorSchemes = new NexusColorSchemesClient(this.HttpClient);
            this.Games = new NexusGamesClient(this.HttpClient);
            this.Mods = new NexusModsClient(this.HttpClient);
            this.ModFiles = new NexusModFilesClient(this.HttpClient);
            this.Users = new NexusUsersClient(this.HttpClient);
        }

        /// <summary>Set the user agent header.</summary>
        /// <param name="userAgent">The user agent to set.</param>
        public NexusClient SetUserAgent(string userAgent)
        {
            this.HttpClient.SetUserAgent(userAgent);
            return this;
        }

        /// <summary>Get metadata about the last request, including rate limits and runtime.</summary>
        /// <returns>Returns the last request metadata, or <c>null</c> if no request has been submitted yet.</returns>
        public RequestMetadata GetLastRequestMetadata()
        {
            return this.LastRequestMetadata;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Track Nexus metadata for an HTTP response.</summary>
        /// <param name="response">The HTTP response from the Nexus API.</param>
        private void OnResponseReceived(IResponse response)
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

            this.LastRequestMetadata = new RequestMetadata
            {
                DailyLimit = GetHeaderValue("x-rl-daily-limit", int.Parse),
                DailyRemaining = GetHeaderValue("x-rl-daily-remaining", int.Parse),
                DailyReset = GetHeaderValue("x-rl-daily-reset", DateTimeOffset.Parse),
                HourlyLimit = GetHeaderValue("x-rl-hourly-limit", int.Parse),
                HourlyRemaining = GetHeaderValue("x-rl-hourly-remaining", int.Parse),
                HourlyReset = GetHeaderValue("x-rl-hourly-reset", DateTimeOffset.Parse),
                LastRequestRuntime = GetHeaderValue("x-runtime", float.Parse)
            };
        }

        /// <summary>Get the default user agent.</summary>
        private string GetDefaultUserAgent()
        {
            Version version = typeof(NexusClient).GetTypeInfo().Assembly.GetName().Version;
            return $"FluentNexus/{version} (+http://github.com/Pathoschild/FluentNexus)";
        }
    }
}
