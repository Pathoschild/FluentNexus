using System.Runtime.Serialization;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A mod publication status.</summary>
    public enum ModStatus
    {
        /// <summary>Not applicable.</summary>
        None,

        /// <summary>The mod is not yet published.</summary>
        [EnumMember(Value = "not_published")]
        NotPublished,

        /// <summary>The mod is published and visible.</summary>
        Published,

        /// <summary>The mod has been hidden by the author.</summary>
        Hidden,

        /// <summary>The mod has been deleted.</summary>
        Wastebinned
    }
}
