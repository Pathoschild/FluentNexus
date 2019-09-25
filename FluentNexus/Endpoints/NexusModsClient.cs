using System.Collections.Generic;
using System.Threading.Tasks;
using Pathoschild.FluentNexus.Framework;
using Pathoschild.FluentNexus.Models;
using Pathoschild.Http.Client;

namespace Pathoschild.FluentNexus.Endpoints
{
    /// <summary>A wrapper for the Nexus mods endpoints.</summary>
    public class NexusModsClient
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
        public NexusModsClient(IClient client)
        {
            this.Client = client;
        }

        /****
        ** Mod lists
        ****/
        /// <summary>Get the mods which were updated within a given period. Cached for 5 minutes.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="period">The period to check. The only supported values are <c>1d</c> (one day), <c>1w</c> (one week), and <c>1m</c> (one month).</param>
        public async Task<ModUpdate[]> GetUpdated(string domainName, string period)
        {
            return await this.Client
                .GetAsync($"v1/games/{domainName}/mods/updated.json")
                .WithArguments(new { period = period })
                .AsArray<ModUpdate>()
                .MakeSyncSafe();
        }

        /// <summary>Get the ten mods most recently added for a specified game.</summary>
        /// <param name="domainName">The game key.</param>
        public async Task<Mod[]> GetLatestAdded(string domainName)
        {
            return await this.Client
                .GetAsync($"v1/games/{domainName}/mods/latest_added.json")
                .AsArray<Mod>()
                .MakeSyncSafe();
        }

        /// <summary>Get the ten mods most recently updated for a specified game.</summary>
        /// <param name="domainName">The game key.</param>
        public async Task<Mod[]> GetLatestUpdated(string domainName)
        {
            return await this.Client
                .GetAsync($"v1/games/{domainName}/mods/latest_updated.json")
                .AsArray<Mod>()
                .MakeSyncSafe();
        }

        /// <summary>Get the ten all-time top trending mods for a specified game.</summary>
        /// <param name="domainName">The game key.</param>
        public async Task<Mod[]> GetTrending(string domainName)
        {
            return await this.Client
                .GetAsync($"v1/games/{domainName}/mods/trending.json")
                .AsArray<Mod>()
                .MakeSyncSafe();
        }

        /// <summary>Get the mods which has a file matching the given MDF hash.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="hash">The MDF file hash.</param>
        public async Task<ModHashResult[]> GetModsByFileHash(string domainName, string hash)
        {
            return await this.Client
                .GetAsync($"v1/games/{domainName}/mods/md5_search/{hash}.json")
                .AsArray<ModHashResult>()
                .MakeSyncSafe();
        }

        /****
        ** Mod info
        ****/
        /// <summary>Get data about a specified mod.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="modID">The unique mod ID.</param>
        public async Task<Mod> GetMod(string domainName, int modID)
        {
            return await this.Client
                .GetAsync($"v1/games/{domainName}/mods/{modID}.json")
                .As<Mod>()
                .MakeSyncSafe();
        }

        /// <summary>Get the change logs for a mod, indexed by version number.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="modID">The unique mod ID.</param>
        public async Task<IDictionary<string, string[]>> GetModChangeLogs(string domainName, int modID)
        {
            return await this.Client
                .GetAsync($"v1/games/{domainName}/mods/{modID}/changelogs.json")
                .As<Dictionary<string, string[]>>()
                .MakeSyncSafe();
        }

        /****
        ** Actions
        ****/
        /// <summary>Add an endorsement for a mod.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="modID">The unique mod ID.</param>
        /// <param name="version">The version to endorse.</param>
        public async Task Endorse(string domainName, int modID, string version)
        {
            await this.Client
                .PostAsync($"v1/games/{domainName}/mods/{modID}/endorse.json")
                .WithBody(new { Version = version })
                .MakeSyncSafe(); // sent as case-sensitive JSON (unlike other endpoints)
        }

        /// <summary>Remove an endorsement for a mod.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="modID">The unique mod ID.</param>
        /// <param name="version">The version to endorse.</param>
        public async Task Unendorse(string domainName, int modID, string version)
        {
            await this.Client
                .PostAsync($"v1/games/{domainName}/mods/{modID}/abstain.json")
                .WithBody(new { Version = version })
                .MakeSyncSafe(); // sent as case-sensitive JSON (unlike other endpoints)
        }
    }
}
