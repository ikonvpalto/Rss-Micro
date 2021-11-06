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
        private readonly string _baseUrl;

        public DownloadManagerProxy(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task CreateAsync(RssSourceManageModel rssSource)
        {
            await _httpClient.InternalPost($"{_baseUrl}/api/rss-sources", rssSource).ConfigureAwait(false);
        }

        public async Task UpdateAsync(RssSourceManageModel rssSource)
        {
            await _httpClient.InternalPut($"{_baseUrl}/api/rss-sources/{rssSource.Guid:D}", rssSource).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid rssSourceGuid)
        {
            await _httpClient.InternalDelete($"{_baseUrl}/api/rss-sources/{rssSourceGuid:D}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<NewsItem>> DownloadNewsAsync(Guid rssSourceGuid)
        {
            return await _httpClient.InternalPost<IEnumerable<NewsItem>>($"{_baseUrl}/api/rss-sources/{rssSourceGuid:D}/news").ConfigureAwait(false);
        }
    }
}
