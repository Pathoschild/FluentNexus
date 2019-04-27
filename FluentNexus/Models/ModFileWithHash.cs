namespace Pathoschild.FluentNexus.Models
{
    /// <summary>A downloadable mod file, with its MD5 hash.</summary>
    public class ModFileWithHash : ModFile
    {
        /// <summary>The MD5 file hash.</summary>
        public string Md5 { get; set; }
    }
}
