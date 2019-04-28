using System.Threading.Tasks;
using Pathoschild.FluentNexus.Framework;
using Pathoschild.FluentNexus.Models;
using Pathoschild.Http.Client;

namespace Pathoschild.FluentNexus.Endpoints
{
    /// <summary>A wrapper for the Nexus color schemes endpoints.</summary>
    public class NexusColorSchemesClient
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
        public NexusColorSchemesClient(IClient client)
        {
            this.Client = client;
        }

        /// <summary>Get a list of color schemes on the Nexus Mods website.</summary>
        public async Task<ColorScheme[]> GetColorSchemes()
        {
            return await this.Client
                .GetAsync("v1/colourschemes.json")
                .AsArray<ColorScheme>()
                .MakeSyncSafe();
        }
    }
}
