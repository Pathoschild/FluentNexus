namespace Pathoschild.FluentNexus.Models
{
    /// <summary>Metadata about the contents of a mod download file if it's an archive.</summary>
    public class ContentPreview
    {
        /// <summary>The files and folders in the archive.</summary>
        public ContentPreviewEntry[] Children { get; set; }
    }
}
