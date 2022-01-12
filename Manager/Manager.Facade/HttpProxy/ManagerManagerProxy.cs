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

        public ManagerManagerProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(JobModel job)
        {
            await _httpClient.InternalPost("/api/manager", job).ConfigureAwait(false);
        }

        public async Task UpdateAsync(JobModel job)
        {
            await _httpClient.InternalPut("/api/manager", job).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid jobGuid)
        {
            await _httpClient.InternalDelete($"/api/manager/{jobGuid:D}").ConfigureAwait(false);
        }
    }
}
