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
        private readonly string _baseUrl;

        public ManagerProviderProxy(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task<JobModel> GetAsync(Guid receiversGuid)
        {
            return await _httpClient.InternalGet<JobModel>($"{_baseUrl}/api/manager/{receiversGuid:D}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<JobModel>> GetAsync()
        {
            return await _httpClient.InternalGet<IEnumerable<JobModel>>($"{_baseUrl}/api/manager").ConfigureAwait(false);
        }

        public async Task EnsureJobPeriodicityIsValidAsync(string jobPeriodicity)
        {
            await _httpClient.InternalGet($"{_baseUrl}/api/manager/ensure-valid?periodicity={jobPeriodicity}").ConfigureAwait(false);
        }
    }
}
