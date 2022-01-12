using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Extensions;
using Downloader.Common.Contracts;
using Downloader.Common.Models;

namespace Downloader.Facade.HttpProxy
{
    public class DownloadManagerProxy : IDownloaderManager
    {
        private readonly HttpClient _httpClient;

        public DownloadManagerProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(RssSourceManageModel rssSource)
        {
            await _httpClient.InternalPost("/api/rss-sources", rssSource).ConfigureAwait(false);
        }

        public async Task UpdateAsync(RssSourceManageModel rssSource)
        {
            await _httpClient.InternalPut($"/api/rss-sources/{rssSource.Guid:D}", rssSource).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid rssSourceGuid)
        {
            await _httpClient.InternalDelete($"/api/rss-sources/{rssSourceGuid:D}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<NewsItem>> DownloadNewsAsync(Guid rssSourceGuid)
        {
            return await _httpClient.InternalPost<IEnumerable<NewsItem>>($"/api/rss-sources/{rssSourceGuid:D}/news").ConfigureAwait(false);
        }

        public async Task<IEnumerable<NewsItem>> DownloadAllNewsAsync()
        {
            return await _httpClient.InternalPost<IEnumerable<NewsItem>>("/api/rss-sources/news").ConfigureAwait(false);
        }
    }
}
