using System;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Extensions;
using Manager.Common.Contracts;
using Manager.Common.Models;

namespace Manager.Facade.HttpProxy
{
    public sealed class ManagerManagerProxy : IManagerManager
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ManagerManagerProxy(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task CreateAsync(JobModel job)
        {
            await _httpClient.InternalPost($"{_baseUrl}/api/manager", job).ConfigureAwait(false);
        }

        public async Task UpdateAsync(JobModel job)
        {
            await _httpClient.InternalPut($"{_baseUrl}/api/manager", job).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid jobGuid)
        {
            await _httpClient.InternalDelete($"{_baseUrl}/api/manager/{jobGuid:D}").ConfigureAwait(false);
        }
    }
}
