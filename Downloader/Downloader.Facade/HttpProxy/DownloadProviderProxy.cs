using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Extensions;
using Downloader.Common.Contracts;
using Downloader.Common.Models;

namespace Downloader.Facade.HttpProxy
{
    public class DownloadProviderProxy : IDownloaderProvider
    {
        private readonly HttpClient _httpClient;

        public DownloadProviderProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RssSourceReadModel> GetAsync(Guid guid)
        {
            return await _httpClient.InternalGet<RssSourceReadModel>($"/api/rss-sources/{guid:D}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<RssSourceReadModel>> GetAsync()
        {
            return await _httpClient.InternalGet<IEnumerable<RssSourceReadModel>>("/api/rss-sources").ConfigureAwait(false);
        }

        public async Task EnsureRssSourceIsValidAsync(string rssSourceUrl)
        {
            //ToDo: rssSourceUrl should be encoded because also may have query params witch may be interpreted as main uri params
            await _httpClient.InternalGet($"/api/rss-sources/ensure-valid?rssSourceUrl={rssSourceUrl}").ConfigureAwait(false);
        }
    }
}
