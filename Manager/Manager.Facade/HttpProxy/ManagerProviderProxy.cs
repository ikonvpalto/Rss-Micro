using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Extensions;
using Manager.Common.Contracts;
using Manager.Common.Models;

namespace Manager.Facade.HttpProxy
{
    public sealed class ManagerProviderProxy : IManagerProvider
    {
        private readonly HttpClient _httpClient;

        public ManagerProviderProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JobModel> GetAsync(Guid receiversGuid)
        {
            return await _httpClient.InternalGet<JobModel>($"/api/manager/{receiversGuid:D}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<JobModel>> GetAsync()
        {
            return await _httpClient.InternalGet<IEnumerable<JobModel>>("/api/manager").ConfigureAwait(false);
        }

        public async Task EnsureJobPeriodicityIsValidAsync(string jobPeriodicity)
        {
            await _httpClient.InternalGet($"/api/manager/ensure-valid?periodicity={jobPeriodicity}").ConfigureAwait(false);
        }
    }
}
