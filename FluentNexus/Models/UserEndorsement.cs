using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A mod endorsement by the user.</summary>
    public class UserEndorsement : IDataModel
    {
        /// <summary>The unique mod ID.</summary>
        [JsonProperty("mod_id")]
        public int ModID { get; set; }

        /// <summary>The mod's game key.</summary>
        [JsonProperty("domain_name")]
        public string DomainName { get; set; }

        /// <summary>When the user endorsed the mod.</summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset Date { get; set; }

        /// <summary>The current mod version when the user endorsed the mod.</summary>
        public string Version { get; set; }

        /// <summary>The mod endorsement status.</summary>
        public EndorsementStatus Status { get; set; }

        /// <inheritdoc />
        [JsonExtensionData]
        public IDictionary<string, object> UnmappedFields { get; set; }
    }
}
