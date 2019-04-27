using Newtonsoft.Json;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>Simplified data about a user.</summary>
    public class UserRef
    {
        /// <summary>The unique user ID.</summary>
        [JsonProperty("member_id")]
        public int MemberID { get; set; }

        /// <summary>The member group ID.</summary>
        [JsonProperty("member_group_id")]
        public int MemberGroupID { get; set; }

        /// <summary>The username.</summary>
        public string Name { get; set; }
    }
}
