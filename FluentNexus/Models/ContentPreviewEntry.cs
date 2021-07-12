using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>Metadata about a file within a download archive file.</summary>
    public class ContentPreviewEntry : IDataModel
    {
        /// <summary>The full path to this entry in the archive, relative to the archive root.</summary>
        public string Path { get; set; }

        /// <summary>The file or folder name.</summary>
        public string Name { get; set; }

        /// <summary>Whether the entry is a file or folder.</summary>
        public ContentPreviewEntryType Type { get; set; }

        /// <summary>A human-readable size for the current file (like <c>500 B</c> or <c>38.9 kB</c>), if applicable. Not defined for a folder.</summary>
        [JsonProperty("size")]
        public string FileSize { get; set; }

        /// <summary>The entries within this entry (e.g. files in this folder), if any.</summary>
        public ContentPreviewEntry[] Children { get; set; } = new ContentPreviewEntry[0];

        /// <inheritdoc />
        [JsonExtensionData]
        public IDictionary<string, object> UnmappedFields { get; set; }
    }
}
