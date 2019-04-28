using System.Threading.Tasks;
using Pathoschild.FluentNexus.Framework;
using Pathoschild.FluentNexus.Models;
using Pathoschild.Http.Client;

namespace Pathoschild.FluentNexus.Endpoints
{
    /// <summary>A wrapper for the Nexus games endpoints.</summary>
    public class NexusGamesClient
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
        public NexusGamesClient(IClient client)
        {
            this.Client = client;
        }

        /// <summary>Get a list of games on Nexus Mods.</summary>
        /// <param name="includeUnapproved">Whether to include unapproved mods.</param>
        public async Task<Game[]> GetGames(bool includeUnapproved = false)
        {
            Game[] games = await this.Client
                .GetAsync("v1/games.json")
                .WithArgument("include_unapproved", includeUnapproved ? 1 : 0)
                .AsArray<Game>()
                .MakeSyncSafe();
            this.NormaliseGameData(games);
            return games;
        }

        /// <summary>Get a game on Nexus Mods.</summary>
        /// <param name="domainName">The game key.</param>
        public async Task<Game> GetGame(string domainName)
        {
            Game game = await this.Client
                .GetAsync($"v1/games/{domainName}.json")
                .As<Game>()
                .MakeSyncSafe();
            this.NormaliseGameData(game);
            return game;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Normalise the data returned by the Nexus API.</summary>
        /// <param name="games">The game records to normalise.</param>
        private void NormaliseGameData(params Game[] games)
        {
            foreach (Game game in games)
            {
                if (game.ApprovedDate?.Year <= 1970)
                    game.ApprovedDate = null; // Nexus API sets this to '1' if it's unapproved

                foreach (GameCategory category in game.Categories)
                {
                    if (category.ParentCategory == 0)
                        category.ParentCategory = null; // Nexus API sets this to 'false' (which becomes 0) if there's no parent category
                }
            }
        }
    }
}
