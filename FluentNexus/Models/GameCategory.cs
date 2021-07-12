using System.Collections.Generic;
using Newtonsoft.Json;
using Pathoschild.FluentNexus.Framework;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A mod category available for a game's mods.</summary>
    public class GameCategory : IDataModel
    {
        /// <summary>The category ID.</summary>
        [JsonProperty("category_id")]
        public int ID { get; set; }

        /// <summary>The category name.</summary>
        public string Name { get; set; }

        /// <summary>The parent category, if any.</summary>
        [JsonProperty("parent_category")]
        [JsonConverter(typeof(NullableIntConverter))]
        public int? ParentCategory { get; set; }

        /// <inheritdoc />
        [JsonExtensionData]
        public IDictionary<string, object> UnmappedFields { get; set; }
    }
}
