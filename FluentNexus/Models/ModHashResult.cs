using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>The result of a mod MD5 hash search.</summary>
    public class ModHashResult : IDataModel
    {
        /// <summary>The matched mod.</summary>
        public Mod Mod { get; set; }

        /// <summary>The matched file details.</summary>
        [JsonProperty("file_details")]
        public ModFileWithHash File { get; set; }

        /// <inheritdoc />
        [JsonExtensionData]
        public IDictionary<string, object> UnmappedFields { get; set; }
    }
}
