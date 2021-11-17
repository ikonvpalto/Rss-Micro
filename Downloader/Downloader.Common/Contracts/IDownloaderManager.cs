using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Downloader.Common.Models;

namespace Downloader.Common.Contracts
{
    public interface IDownloaderManager
    {
        Task CreateAsync(RssSourceManageModel rssSource);

        Task UpdateAsync(RssSourceManageModel rssSource);

        Task DeleteAsync(Guid rssSourceGuid);

        Task<IEnumerable<NewsItem>> DownloadNewsAsync(Guid rssSourceGuid);

        Task<IEnumerable<NewsItem>> DownloadAllNewsAsync();
    }
}
