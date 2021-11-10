using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Downloader.Common.Models;

namespace Downloader.Common.Contracts
{
    public interface IDownloaderProvider
    {
        Task<RssSourceReadModel> GetAsync(Guid guid);

        Task<IEnumerable<RssSourceReadModel>> GetAsync();

        Task EnsureRssSourceValidAsync(string rssSourceUrl);
    }
}
