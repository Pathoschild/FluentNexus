using System.Runtime.Serialization;

namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A mod file category.</summary>
    public enum FileCategory
    {
        /// <summary>A main file.</summary>
        [EnumMember(Value = "MAIN")]
        Main = 1,

        /// <summary>An update file.</summary>
        [EnumMember(Value = "UPDATE")]
        Update = 2,

        /// <summary>An optional file.</summary>
        [EnumMember(Value = "OPTIONAL")]
        Optional = 3,

        /// <summary>An old file.</summary>
        [EnumMember(Value = "OLD_VERSION")]
        Old = 4,

        /// <summary>A miscellaneous file.</summary>
        [EnumMember(Value = "MISCELLANEOUS")]
        Miscellaneous = 5,

        /// <summary>A file which has been fully deleted by the mod author or Nexus Mods, so it can no longer be downloaded from the web UI or API.</summary>
        Deleted = 6,

        /// <summary>A file archived by the mod author, so download links are no longer shown in the web UI but it can still be downloaded through the API.</summary>
        [EnumMember(Value = "ARCHIVED")]
        Archived = 7
    }
}
