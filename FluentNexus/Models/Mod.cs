using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A Nexus mod model.</summary>
    public class Mod : IDataModel
    {
        /// <summary>The mod name.</summary>
        public string Name { get; set; }

        /// <summary>The short mod summary.</summary>
        public string Summary { get; set; }

        /// <summary>The long mod description in BBCode format.</summary>
        public string Description { get; set; }

        /// <summary>The URL for the main image shown in thumbnails.</summary>
        [JsonProperty("picture_url")]
        public Uri PictureUrl { get; set; }

        /// <summary>The unique mod ID.</summary>
        [JsonProperty("mod_id")]
        public int ModID { get; set; }

        /// <summary>The unique game ID.</summary>
        [JsonProperty("game_id")]
        public int GameID { get; set; }

        /// <summary>The game key.</summary>
        [JsonProperty("domain_name")]
        public string DomainName { get; set; }

        /// <summary>The mod category ID.</summary>
        [JsonProperty("category_id")]
        public int CategoryID { get; set; }

        /// <summary>The mod version number.</summary>
        public string Version { get; set; }

        /// <summary>When the mod was created.</summary>
        [JsonProperty("created_timestamp")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset Created { get; set; }

        /// <summary>When the mod was created.</summary>
        [JsonProperty("updated_timestamp")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset Updated { get; set; }

        /// <summary>The user-defined 'created by' author name.</summary>
        public string Author { get; set; }

        /// <summary>The mod author's username.</summary>
        [JsonProperty("uploaded_by")]
        public string UploadedBy { get; set; }

        /// <summary>The mod author's profile URL.</summary>
        [JsonProperty("uploaded_users_profile_url")]
        public Uri UploadedByProfileUrl { get; set; }

        /// <summary>Whether the mod contains adult content such as gore, nudity, or extreme violence.</summary>
        [JsonProperty("contains_adult_content")]
        public bool ContainsAdultContent { get; set; }

        /// <summary>The mod publication status.</summary>
        public ModStatus Status { get; set; }

        /// <summary>Whether the mod is published and available.</summary>
        [JsonProperty("available")]
        public bool IsAvailable { get; set; }

        /// <summary>The user who uploaded them od.</summary>
        public UserRef User { get; set; }

        /// <summary>Whether the current user is allowed to endorse this mod.</summary>
        [JsonProperty("allow_rating")]
        public bool AllowRating { get; set; }

        /// <summary>The user's endorsement status with this mod, or <c>null</c> if not applicable.</summary>
        public EndorsementRef Endorsement { get; set; }

        /// <summary>The number of endorsements given by users for this mod.</summary>
        [JsonProperty("endorsement_count")]
        public int EndorsementCount { get; set; }

        /// <inheritdoc />
        [JsonExtensionData]
        public IDictionary<string, object> UnmappedFields { get; set; }
    }
}
