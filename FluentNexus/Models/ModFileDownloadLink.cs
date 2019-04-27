using System;
using Newtonsoft.Json;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A mod file download link.</summary>
    public class ModFileDownloadLink
    {
        /// <summary>The full name of the CDN serving the file.</summary>
        [JsonProperty("name")]
        public string CdnName { get; set; }

        /// <summary>The short name of the CDN serving the file.</summary>
        [JsonProperty("short_name")]
        public string CdnShortName { get; set; }

        /// <summary>The download URL.</summary>
        public Uri Uri { get; set; }
    }
}
