using System.Collections.Generic;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A data model returned by the Nexus client.</summary>
    public interface IDataModel
    {
        /// <summary>The fields returned by the Nexus API which weren't mapped to one of the known properties, if any.</summary>
        /// <remarks>This is only meant to support cases where the client models haven't been updated for an API change. You shouldn't need to use this property in most cases. This may be <c>null</c> if empty.</remarks>
        IDictionary<string, object> UnmappedFields { get; set; }
    }
}
