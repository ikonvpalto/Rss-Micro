using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Extensions;
using Downloader.Common.Models;
using Filter.Common.Contracts;
using Filter.Common.Models;

namespace Filter.Facade.HttpProxy
{
    public sealed class FilterProviderProxy : IFilterProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public FilterProviderProxy(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task<NewsFilterModel> GetAsync(Guid filterGuid)
        {
            return await _httpClient.InternalGet<NewsFilterModel>($"{_baseUrl}/api/filter/{filterGuid:D}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<NewsFilterModel>> GetAsync()
        {
            return await _httpClient.InternalGet<IEnumerable<NewsFilterModel>>($"{_baseUrl}/api/filter").ConfigureAwait(false);
        }

        public async Task<IEnumerable<NewsItem>> FilterNewsAsync(Guid filterGuid, IEnumerable<NewsItem> news)
        {
            return await _httpClient.InternalPost<IEnumerable<NewsItem>>($"{_baseUrl}/api/filter/{filterGuid:D}/filter-news", news).ConfigureAwait(false);
        }

        public async Task EnsureFiltersValidAsync(IEnumerable<string> filters)
        {
            await _httpClient.InternalPost($"{_baseUrl}/api/filter/ensure-valid", filters).ConfigureAwait(false);
        }
    }
}
