using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A list of files for a mod.</summary>
    public class ModFileList : IDataModel
    {
        /// <summary>The matched file details.</summary>
        public ModFile[] Files { get; set; }

        /// <summary>The update relationships between files (i.e. a record of the uploader marking each file as a newer version of a previous one, if they did).</summary>
        [JsonProperty("file_updates")]
        public ModFileUpdate[] FileUpdates { get; set; }

        /// <inheritdoc />
        [JsonExtensionData]
        public IDictionary<string, object> UnmappedFields { get; set; }
    }
}
