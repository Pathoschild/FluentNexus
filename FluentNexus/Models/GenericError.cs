namespace Pathoschild.FluentNexus.Models
{
    /// <summary>The generic model for a Nexus Mods error.</summary>
    public class GenericError
    {
        /// <summary>The HTTP status code.</summary>
        public int Code { get; set; }

        /// <summary>The error message indicating what went wrong.</summary>
        public string Message { get; set; }
    }
}
