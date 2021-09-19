using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Pathoschild.FluentNexus.Endpoints;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A downloadable mod file.</summary>
    public class ModFile : IDataModel
    {
        /// <summary>The unique file ID.</summary>
        [JsonProperty("file_id")]
        public int FileID { get; set; }

        /// <summary>The download name.</summary>
        public string Name { get; set; }

        /// <summary>The download description.</summary>
        public string Description { get; set; }

        /// <summary>The file version number.</summary>
        [JsonProperty("version")]
        public string FileVersion { get; set; }

        /// <summary>The mod version at the time the file was uploaded.</summary>
        [JsonProperty("mod_version")]
        public string ModVersion { get; set; }

        /// <summary>The download filename.</summary>
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        /// <summary>The file category.</summary>
        [JsonProperty("category_id")]
        public FileCategory Category { get; set; }

        /// <summary>Whether the file is marked as the primary download.</summary>
        [JsonProperty("is_primary")]
        public bool IsPrimary { get; set; }

        /// <summary>The file size in kilobytes.</summary>
        [Obsolete("Use " + nameof(SizeInKilobytes) + " instead.")]
        public long Size { get; set; }

        /// <summary>The file size in kilobytes.</summary>
        [JsonProperty("size_kb")]
        public long SizeInKilobytes { get; set; }

        /// <summary>The file size in bytes, if available.</summary>
        [JsonProperty("size_in_bytes")]
        public long? SizeInBytes { get; set; }

        /// <summary>When the file was uploaded.</summary>
        [JsonProperty("uploaded_timestamp")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset UploadedTimestamp { get; set; }

        /// <summary>The URL to the external virus scan results.</summary>
        [JsonProperty("external_virus_scan_url")]
        public Uri ExternalVirusScanUri { get; set; }

        /// <summary>The HTML change logs, if any.</summary>
        [JsonProperty("changelog_html")]
        public string ChangeLog { get; set; }

        /// <summary>The URL to a JSON file which lists the contents for the mod file, if it's an archive file. The file can be fetched using <see cref="NexusModFilesClient.GetContentPreview"/>.</summary>
        [JsonProperty("content_preview_link")]
        public Uri ContentPreviewLink { get; set; }

        /// <inheritdoc />
        [JsonExtensionData]
        public IDictionary<string, object> UnmappedFields { get; set; }
    }
}
