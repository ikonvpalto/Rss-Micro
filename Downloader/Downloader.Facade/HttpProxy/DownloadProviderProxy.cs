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
        private readonly string _baseUrl;

        public DownloadProviderProxy(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task<IEnumerable<RssSourceReadModel>> GetAsync()
        {
            return await _httpClient.InternalGet<IEnumerable<RssSourceReadModel>>($"{_baseUrl}/api/rss-sources").ConfigureAwait(false);
        }

        public async Task EnsureRssSourceValidAsync(string rssSourceUrl)
        {
            //ToDo: rssSourceUrl should be encoded because also may have query params witch may be interpreted as main uri params
            await _httpClient.InternalGet($"{_baseUrl}/api/rss-sources/ensure-valid?rssSourceUrl={rssSourceUrl}").ConfigureAwait(false);
        }
    }
}
