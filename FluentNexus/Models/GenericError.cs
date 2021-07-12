using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>The generic model for a Nexus Mods error.</summary>
    public class GenericError : IDataModel
    {
        /// <summary>The HTTP status code.</summary>
        public int Code { get; set; }

        /// <summary>The error message indicating what went wrong.</summary>
        public string Message { get; set; }

        /// <inheritdoc />
        [JsonExtensionData]
        public IDictionary<string, object> UnmappedFields { get; set; }
    }
}
