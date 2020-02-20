using System;
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

        /// <summary>The <see cref="DateTimeOffset"/> ticks which match Unix epoch.</summary>
        private static readonly long UnixEpochTicks =
#if NETSTANDARD
            DateTimeOffset.FromUnixTimeMilliseconds(0).Ticks;
#else
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeSpan.Zero).Ticks;
#endif


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
                // normalise approval date
                // Nexus API sets it to '0' (unapproved), '1' (approved before the date was tracked), or a valid date
                game.IsApproved = game.ApprovedDate?.Ticks > NexusGamesClient.UnixEpochTicks;
                if (game.ApprovedDate?.Year <= 1970)
                    game.ApprovedDate = null;
            }
        }
    }
}
