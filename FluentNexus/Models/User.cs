using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>User login metadata.</summary>
    public class User : IDataModel
    {
        /// <summary>The unique user ID.</summary>
        [JsonProperty("user_id")]
        public int UserID { get; set; }

        /// <summary>The user's API authentication key.</summary>
        public string Key { get; set; }

        /// <summary>The username.</summary>
        public string Name { get; set; }

        /// <summary>The user's email address.</summary>
        public string Email { get; set; }

        /// <summary>The URL of the user's avatar.</summary>
        [JsonProperty("profile_url")]
        public string ProfileUrl { get; set; }

        /// <summary>Whether the user has a premium Nexus account.</summary>
        [JsonProperty("is_premium")]
        public bool IsPremium { get; set; }

        /// <summary>Whether the user has a supporter Nexus account.</summary>
        [JsonProperty("is_supporter")]
        public bool IsSupporter { get; set; }

        /// <inheritdoc />
        [JsonExtensionData]
        public IDictionary<string, object> UnmappedFields { get; set; }
    }
}
