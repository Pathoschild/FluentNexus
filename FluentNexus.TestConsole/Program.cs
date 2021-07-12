using System;
using System.Linq;
using System.Threading.Tasks;
using FluentNexus.TestConsole.Framework;
using Pathoschild.FluentNexus;
using Pathoschild.FluentNexus.Models;

namespace FluentNexus.TestConsole
{
    /// <summary>A test app to simplify running quick tests against the Nexus API.</summary>
    public class Program
    {
        /*********
        ** Fields
        *********/
        /// <summary>The Nexus API key.</summary>
        private const string ApiKey = "";


        /*********
        ** Public methods
        *********/
        /// <summary>The app entry point.</summary>
        public static async Task Main()
        {
            // init client
            if (string.IsNullOrWhiteSpace(Program.ApiKey))
                throw new InvalidOperationException($"You must set the {nameof(Program)}.{nameof(Program.ApiKey)} field to your personal Nexus API key to use this test console.");
            NexusClient nexus = new(Program.ApiKey, "Pathoschild", "1.0.0");

            // users
            //User user = await nexus.Users.ValidateAsync().DumpAsync("user");
            //await nexus.Users.TrackMod("stardewvalley", 999);
            //await nexus.Users.UntrackMod("stardewvalley", 999);
            //var trackedMods = await nexus.Users.GetTrackedMods().DumpAsync("tracked mods");
            //var endorsements = await nexus.Users.GetEndorsements().DumpAsync("endorsements");

            // mods
            //var lastAddedMods = await nexus.Mods.GetLatestAdded("stardewvalley").DumpAsync("last added mods");
            //var lastUpdatedMods = await nexus.Mods.GetLatestUpdated("stardewvalley").DumpAsync("last updated mods");
            //var updatedMods = await nexus.Mods.GetUpdated("stardewvalley", "1m").DumpAsync("updated mods within 1 month");
            //var trendingMods = await nexus.Mods.GetTrending("stardewvalley").DumpAsync("all-time trending mods");
            //var modsByHash = await nexus.Mods.GetModsByFileHash("stardewvalley", "8d5ab896282c948a5a1ef7636a0ea7da").DumpAsync("mod info by MD5 hash");
            //var mod = await nexus.Mods.GetMod("stardewvalley", 3753).DumpAsync("mod info");
            //var changelogs = await nexus.Mods.GetModChangeLogs("stardewvalley", 3753).DumpAsync("mod change logs");
            //await nexus.Mods.Endorse("stardewvalley", 3753, "1.0.11");
            //await nexus.Mods.Unendorse("stardewvalley", 3753, "1.0.11");

            // mod files
            //var modFiles = await nexus.ModFiles.GetModFiles("stardewvalley", 2400).DumpAsync("mod files");
            //var modFile = await nexus.ModFiles.GetModFile("stardewvalley", 2400, modFiles.Files.First().FileID).DumpAsync("mod file");
            //var downloadLinkNonPremium = await nexus.ModFiles.GetDownloadLinks("stardewvalley", 1915, 15696, "G4qE27bUlVLE9pSlHXlX6w", 1556509738).DumpAsync("mod file download link (non-premium)");

            // content preview
            //var contentPreview = await nexus.ModFiles.GetContentPreview(modFile.ContentPreviewLink).DumpAsync("mod file content preview");

            // games
            //var games = await nexus.Games.GetGames(includeUnapproved: false).DumpAsync("games");
            //var game = await nexus.Games.GetGame("stardewvalley").DumpAsync("game");

            // color schemes
            //var colorSchemes = await nexus.ColorSchemes.GetColorSchemes().DumpAsync("color schemes");

            // metadata
            //var rateLimits = nexus.GetRateLimits().DumpAsync("rate limits");

            // will throw exception without premium account
            //var downloadLinkPremium = await nexus.ModFiles.GetDownloadLinks("stardewvalley", 1915, 15696).DumpAsync("mod file download link (premium)");
        }
    }
}
