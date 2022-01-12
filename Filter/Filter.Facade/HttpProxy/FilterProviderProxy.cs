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

        public FilterProviderProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<NewsFilterModel> GetAsync(Guid filterGuid)
        {
            return await _httpClient.InternalGet<NewsFilterModel>($"/api/filter/{filterGuid:D}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<NewsFilterModel>> GetAsync()
        {
            return await _httpClient.InternalGet<IEnumerable<NewsFilterModel>>("/api/filter").ConfigureAwait(false);
        }

        public async Task<IEnumerable<NewsItem>> FilterNewsAsync(Guid filterGuid, IEnumerable<NewsItem> news)
        {
            return await _httpClient.InternalPost<IEnumerable<NewsItem>>($"/api/filter/{filterGuid:D}/filter-news", news).ConfigureAwait(false);
        }

        public async Task EnsureFiltersIsValidAsync(IEnumerable<string> filters)
        {
            await _httpClient.InternalPost("/api/filter/ensure-valid", filters).ConfigureAwait(false);
        }
    }
}
