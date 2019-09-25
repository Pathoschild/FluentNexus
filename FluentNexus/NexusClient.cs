using System;
using System.Net;
using System.Reflection;
#if !NET45
using System.Runtime.InteropServices;
#endif
using System.Text.RegularExpressions;
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
        /// <summary>Whether authentication failed for the last API request.</summary>
        private bool LastAuthenticationFailed;

        /// <summary>Provides access to the Nexus Mods rate limits with utility methods. This is <c>null</c> if no request was received yet, or if <see cref="LastAuthenticationFailed"/>.</summary>
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
        /// <param name="appName">An arbitrary name for the app/script using the client, reported to the Nexus Mods API and used in the user agent.</param>
        /// <param name="appVersion">An arbitrary version number for the <paramref name="appName"/> (ideally a semantic version).</param>
        /// <param name="baseUrl">The base URL for the API.</param>
        public NexusClient(string apiKey, string appName, string appVersion, string baseUrl = "https://api.nexusmods.com")
        {
            // validate
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("The API key is required.", nameof(apiKey));
            if (string.IsNullOrWhiteSpace(appName))
                throw new ArgumentException("The app name is required.", nameof(appName));
            if (string.IsNullOrWhiteSpace(appVersion))
                throw new ArgumentException("The app version is required.", nameof(appVersion));
            if (Regex.IsMatch(appName, @"[\(\)\/]"))
                throw new ArgumentException("The app name can't contain parentheses or forward slashes.", nameof(appName));
            if (Regex.IsMatch(appVersion, @"[\(\)\/]"))
                throw new ArgumentException("The app version can't contain parentheses or forward slashes.", nameof(appVersion));

            // init client
            this.HttpClient = new FluentClient(baseUrl)
                .SetUserAgent(this.GetDefaultUserAgent(appName, appVersion))
                .AddDefault(p => p
                    .WithHeader("apiKey", apiKey)
                    .WithHeader("Application-Name", appName)
                    .WithHeader("Application-Version", appVersion)
                );

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
        /// <exception cref="InvalidOperationException">Can't retrieve rate limits because API authentication failed.</exception>
        public async Task<IRateLimitManager> GetRateLimits()
        {
            // refresh data
            if (!this.LastAuthenticationFailed && (this.RateLimits == null || this.RateLimits.IsOutdated()))
                await this.Users.ValidateAsync();

            // return rate limits if available
            if (this.LastAuthenticationFailed)
                throw new InvalidOperationException("Can't retrieve rate limits because API authentication failed.");
            return this.RateLimits;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Track Nexus metadata for an HTTP response.</summary>
        /// <param name="response">The HTTP response from the Nexus API.</param>
        private void OnResponseReceived(IResponse response)
        {
            // ignore non-API query (e.g. content preview URLs)
            if (!response.Message.Headers.Contains("x-rl-daily-limit") && response.Message.RequestMessage.RequestUri.Host != this.HttpClient.BaseClient.BaseAddress.Host)
                return;

            // handle authentication failure
            if (response.Status == HttpStatusCode.Unauthorized)
            {
                this.RateLimits = null;
                this.LastAuthenticationFailed = true;
                return;
            }

            // handle success
            this.LastAuthenticationFailed = false;
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
        /// <param name="appName">A unique name for the app or script using the client, reported to the Nexus Mods API and used in the user agent.</param>
        /// <param name="appVersion">A version number for the <paramref name="appName"/> (ideally a semantic version).</param>
        private string GetDefaultUserAgent(string appName, string appVersion)
        {
            string platformStr =
#if NET45
                $"{Environment.OSVersion.VersionString}; {(Environment.Is64BitProcess ? "x64" : "x86")}";
#else
                $"{RuntimeInformation.OSDescription?.Trim()}; {RuntimeInformation.OSArchitecture}; {RuntimeInformation.FrameworkDescription}";
#endif
            Version clientVersion = typeof(NexusClient).GetTypeInfo().Assembly.GetName().Version;
            return $"{appName}/{appVersion} ({platformStr}) FluentNexus/{clientVersion.Major}.{clientVersion.Minor}.{clientVersion.Build}";
        }
    }
}
