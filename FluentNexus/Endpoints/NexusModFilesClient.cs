using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Pathoschild.FluentNexus.Framework;
using Pathoschild.FluentNexus.Models;
using Pathoschild.Http.Client;

namespace Pathoschild.FluentNexus.Endpoints
{
    /// <summary>A wrapper for the Nexus mod files endpoints.</summary>
    public class NexusModFilesClient
    {
        /*********
        ** Fields
        *********/
        /// <summary>The Nexus API client.</summary>
        private readonly IClient Client;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="client">The underlying HTTP client.</param>
        public NexusModFilesClient(IClient client)
        {
            this.Client = client;
        }

        /****
        ** Files
        ****/
        /// <summary>Get all files a specific mod.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="modID">The unique mod ID.</param>
        /// <param name="categories">The categories for which to fetch files, or empty to fetch all files (including archived files).</param>
        public async Task<ModFileList> GetModFiles(string domainName, int modID, params FileCategory[] categories)
        {
            // format category list
            categories = categories.Where(p => p != FileCategory.Deleted).Distinct().ToArray();
            string categoryList = categories.Any() ? string.Join(",", categories.Select(this.GetCategoryFilter)) : null;

            // send request
            return await this.Client
                .GetAsync($"v1/games/{domainName}/mods/{modID}/files.json")
                .WithOptions(ignoreNullArguments: true)
                .WithArgument("category", categoryList)
                .As<ModFileList>()
                .MakeSyncSafe();
        }

        /// <summary>Get a specific mod file.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="modID">The unique mod ID.</param>
        /// <param name="fileID">The unique file ID.</param>
        public async Task<ModFile> GetModFile(string domainName, int modID, int fileID)
        {
            return await this.Client
                .GetAsync($"v1/games/{domainName}/mods/{modID}/files/{fileID}.json")
                .As<ModFile>()
                .MakeSyncSafe();
        }

        /****
        ** Download links
        ****/
        /// <summary>Get the download links for a mod file. This may return multiple results if the file is available from different CDNs. This overload is only available when authenticated as a premium Nexus account.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="modID">The unique mod ID.</param>
        /// <param name="fileID">The unique file ID.</param>
        public async Task<ModFileDownloadLink[]> GetDownloadLinks(string domainName, int modID, int fileID)
        {
            return await this.Client
                .GetAsync($"v1/games/{domainName}/mods/{modID}/files/{fileID}/download_link.json")
                .AsArray<ModFileDownloadLink>()
                .MakeSyncSafe();
        }

        /// <summary>Get the download links for a mod file. This may return multiple results if the file is available from different CDNs.</summary>
        /// <param name="domainName">The game key.</param>
        /// <param name="modID">The unique mod ID.</param>
        /// <param name="fileID">The unique file ID.</param>
        /// <param name="nxmKey">The <c>key</c> argument extracted from the nxm:// link on the Nexus website.</param>
        /// <param name="nxmExpiry">The <c>expires</c> argument extracted from the nxm:// link on the Nexus website.</param>
        public async Task<ModFileDownloadLink[]> GetDownloadLinks(string domainName, int modID, int fileID, string nxmKey, int nxmExpiry)
        {
            return await this.Client
                .GetAsync($"v1/games/{domainName}/mods/{modID}/files/{fileID}/download_link.json")
                .WithArguments(new
                {
                    key = nxmKey,
                    expires = nxmExpiry.ToString(CultureInfo.InvariantCulture)
                })
                .AsArray<ModFileDownloadLink>()
                .MakeSyncSafe();
        }

        /****
        ** File preview
        ****/
        /// <summary>Get a preview of the contents of a mod file, if any.</summary>
        /// <param name="url">The content preview URL from the <see cref="ModFile.ContentPreviewLink"/> field returned by <see cref="GetModFiles"/> or <see cref="GetModFile"/>.</param>
        public async Task<ContentPreview> GetContentPreview(Uri url)
        {
            if (url == null)
                return null;

            return await this.Client
                .GetAsync(url.ToString())
                .As<ContentPreview>();
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the filter name for a category.</summary>
        /// <param name="category">The file category.</param>
        private string GetCategoryFilter(FileCategory category)
        {
            if (category == FileCategory.Deleted)
                throw new NotSupportedException($"Can't use the {category} category as a filter.");

            EnumMemberAttribute attribute = typeof(FileCategory).GetRuntimeField(category.ToString()).GetCustomAttribute<EnumMemberAttribute>();
            if (attribute == null)
                throw new NotSupportedException($"Invalid category filter '{category}'.");

            return attribute.Value.ToLower();
        }
    }
}
