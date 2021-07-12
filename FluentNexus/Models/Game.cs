using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A Nexus game model.</summary>
    public class Game : IDataModel
    {
        /// <summary>The unique game ID.</summary>
        public int ID { get; set; }

        /// <summary>The unique game key.</summary>
        [JsonProperty("domain_name")]
        public string DomainName { get; set; }

        /// <summary>The game name.</summary>
        public string Name { get; set; }

        /// <summary>The URL to the Nexus Mods front page for this game.</summary>
        [JsonProperty("nexusmods_url")]
        public Uri ModsUrl { get; set; }

        /// <summary>The URL to the Nexus Mods forum for this game.</summary>
        [JsonProperty("forum_url")]
        public Uri ForumUrl { get; set; }

        /// <summary>A brief description of the game genre, like 'Adventure' or 'Dungeon crawl'.</summary>
        public string Genre { get; set; }

        /// <summary>The number of mods created for the game.</summary>
        public long Mods { get; set; }

        /// <summary>The number of files uploaded for the game's mods.</summary>
        [JsonProperty("file_count")]
        public long ModFiles { get; set; }

        /// <summary>The number of file downloads for the game's mods.</summary>
        public long Downloads { get; set; }

        /// <summary>The number of page views for the game's mods.</summary>
        [JsonProperty("file_views")]
        public long Views { get; set; }

        /// <summary>The number of users who created a mod page for the game.</summary>
        public long Authors { get; set; }

        /// <summary>The number of mod endorsements for the game's mods.</summary>
        [JsonProperty("file_endorsements")]
        public long Endorsements { get; set; }

        /// <summary>The mod categories defined for this game.</summary>
        public GameCategory[] Categories { get; set; }

        /// <summary>Whether the game is approved and available on Nexus Mods.</summary>
        public bool IsApproved { get; set; }

        /// <summary>When the game became approved and available on Nexus Mods, if available. This may be null for very old approved games.</summary>
        [JsonProperty("approved_date")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset? ApprovedDate { get; set; }

        /// <inheritdoc />
        [JsonExtensionData]
        public IDictionary<string, object> UnmappedFields { get; set; }
    }
}
