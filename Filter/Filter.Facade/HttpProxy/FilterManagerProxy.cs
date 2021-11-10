using System;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Extensions;
using Filter.Common.Contracts;
using Filter.Common.Models;

namespace Filter.Facade.HttpProxy
{
    public sealed class FilterManagerProxy : IFilterManager
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public FilterManagerProxy(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task CreateAsync(NewsFilterModel filter)
        {
            await _httpClient.InternalPost($"{_baseUrl}/api/filter", filter).ConfigureAwait(false);
        }

        public async Task UpdateAsync(NewsFilterModel filter)
        {
            await _httpClient.InternalPut($"{_baseUrl}/api/filter", filter).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid filterGuid)
        {
            await _httpClient.InternalDelete($"{_baseUrl}/api/filter/{filterGuid:D}").ConfigureAwait(false);
        }
    }
}
