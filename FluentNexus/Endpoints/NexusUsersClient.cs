using System.Net;
using System.Threading.Tasks;
using Pathoschild.FluentNexus.Framework;
using Pathoschild.FluentNexus.Models;
using Pathoschild.Http.Client;

namespace Pathoschild.FluentNexus.Endpoints
{
    /// <summary>A wrapper for the Nexus users endpoints.</summary>
    public class NexusUsersClient
    {
        /*********
        ** Fields
        *********/
        /// <summary>The Nexus API client.</summary>
        private readonly IClient Client;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="client">The underlying HTTP client.</param>
        public NexusUsersClient(IClient client)
        {
            this.Client = client;
        }

        /// <summary>Get the user details if the API key is valid.</summary>
        /// <returns>Returns the user details if valid, else <c>null</c>.</returns>
        public async Task<User> ValidateAsync()
        {
            return await this.Client
                .GetAsync("v1/users/validate.json")
                .As<User>()
                .MakeSyncSafe();
        }

        /// <summary>Remove a mod from the user's tracked mods list, if it's tracked.</summary>
        public async Task<UserEndorsement[]> GetEndorsements()
        {
            return await this.Client
                .GetAsync("v1/user/endorsements.json")
                .AsArray<UserEndorsement>()
                .MakeSyncSafe();
        }

        /// <summary>Fetch the mods being tracked by the user.</summary>
        public async Task<UserTrackedMod[]> GetTrackedMods()
        {
            return await this.Client
                .GetAsync("v1/user/tracked_mods.json")
                .AsArray<UserTrackedMod>()
                .MakeSyncSafe();
        }

        /// <summary>Add a mod to the user's tracked mods list.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="modID">The mod ID.</param>
        /// <returns>Returns whether the mod is now being tracked (false if the mod doesn't exist).</returns>
        public async Task<bool> TrackMod(string domainName, int modID)
        {
            IResponse response = await this.Client
                .PostAsync("v1/user/tracked_mods.json")
                .WithArgument("domain_name", domainName)
                .WithBody(new { mod_id = modID })
                .WithOptions(ignoreHttpErrors: true)
                .MakeSyncSafe();

            if (response.IsSuccessStatusCode)
                return true;
            if (response.Status == HttpStatusCode.NotFound)
                return false;
            throw new ApiException(response, $"The API query failed with status code {response.Message.StatusCode}: {response.Message.ReasonPhrase}");
        }

        /// <summary>Remove a mod from the user's tracked mods list, if it's tracked.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="modID">The mod ID.</param>
        public async Task UntrackMod(string domainName, int modID)
        {
            IResponse response = await this.Client
                .DeleteAsync("v1/user/tracked_mods.json")
                .WithArgument("domain_name", domainName)
                .WithBody(new { mod_id = modID })
                .WithOptions(ignoreHttpErrors: true)
                .MakeSyncSafe();

            if (!response.IsSuccessStatusCode && response.Status != HttpStatusCode.NotFound)
                throw new ApiException(response, $"The API query failed with status code {response.Message.StatusCode}: {response.Message.ReasonPhrase}");
        }
    }
}
