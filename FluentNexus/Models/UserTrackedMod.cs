using Newtonsoft.Json;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A mod tracked by the user.</summary>
    public class UserTrackedMod
    {
        /// <summary>The unique mod ID.</summary>
        [JsonProperty("mod_id")]
        public int ModID { get; set; }

        /// <summary>The mod's game key.</summary>
        [JsonProperty("domain_name")]
        public string DomainName { get; set; }
    }
}
