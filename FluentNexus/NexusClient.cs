using System;
using System.Reflection;
using System.Threading.Tasks;
using Pathoschild.FluentNexus.Endpoints;
using Pathoschild.FluentNexus.Framework;
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
        /// <summary>Provides access to the Nexus Mods rate limits with utility methods.</summary>
        private RateLimitManager RateLimits;


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
            this.HttpClient.Filters.Add(new ResponseCallbackFilter(this.OnResponseReceived)); // must be set before the error filter, so we can update rate limits if possible
            this.HttpClient.Filters.Add(new NexusErrorFilter());

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

        /// <summary>Get metadata about the API rate limits from the last response. If no request has been sent yet, this sends an auth validation request (which doesn't consume rate limits).</summary>
        public async Task<IRateLimitManager> GetRateLimits()
        {
            if (this.RateLimits == null || this.RateLimits.IsOutdated())
                await this.Users.ValidateAsync();

            return this.RateLimits;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Track Nexus metadata for an HTTP response.</summary>
        /// <param name="response">The HTTP response from the Nexus API.</param>
        private void OnResponseReceived(IResponse response)
        {
            if (this.RateLimits == null)
            {
                var rateLimits = new RateLimitManager();
                rateLimits.UpdateFrom(response);
                this.RateLimits = rateLimits;
            }
            else
                this.RateLimits.UpdateFrom(response);
        }

        /// <summary>Get the default user agent.</summary>
        private string GetDefaultUserAgent()
        {
            Version version = typeof(NexusClient).GetTypeInfo().Assembly.GetName().Version;
            return $"FluentNexus/{version} (+http://github.com/Pathoschild/FluentNexus)";
        }
    }
}
