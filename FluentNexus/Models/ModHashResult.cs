using Newtonsoft.Json;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>The result of a mod MD5 hash search.</summary>
    public class ModHashResult
    {
        /// <summary>The matched mod.</summary>
        public Mod Mod { get; set; }

        /// <summary>The matched file details.</summary>
        [JsonProperty("file_details")]
        public ModFileWithHash File { get; set; }
    }
}
