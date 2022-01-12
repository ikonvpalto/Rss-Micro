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

        public FilterManagerProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(NewsFilterModel filter)
        {
            await _httpClient.InternalPost("/api/filter", filter).ConfigureAwait(false);
        }

        public async Task UpdateAsync(NewsFilterModel filter)
        {
            await _httpClient.InternalPut("/api/filter", filter).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid filterGuid)
        {
            await _httpClient.InternalDelete($"/api/filter/{filterGuid:D}").ConfigureAwait(false);
        }
    }
}
