using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>Simplified data about a mod endorsement.</summary>
    public class EndorsementRef : IDataModel
    {
        /// <summary>The mod endorsement status.</summary>
        [JsonProperty("endorse_status")]
        public EndorsementStatus EndorseStatus { get; set; }

        /// <summary>When the user endorsed the mod (if they did).</summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>The current mod version when the user endorsed the mod.</summary>
        public string Version { get; set; }

        /// <inheritdoc />
        [JsonExtensionData]
        public IDictionary<string, object> UnmappedFields { get; set; }
    }
}
