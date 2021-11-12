using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Extensions;
using Sender.Common.Contracts;
using Sender.Common.Models;

namespace Sender.Facade.HttpProxy
{
    public sealed class SenderProviderProxy : ISenderProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public SenderProviderProxy(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task<ReceiversModel> GetAsync(Guid receiversGuid)
        {
            return await _httpClient.InternalGet<ReceiversModel>($"{_baseUrl}/api/sender/{receiversGuid:D}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<ReceiversModel>> GetAsync()
        {
            return await _httpClient.InternalGet<IEnumerable<ReceiversModel>>($"{_baseUrl}/api/sender").ConfigureAwait(false);
        }

        public async Task EnsureReceiversIsValidAsync(IEnumerable<string> receiverEmails)
        {
            await _httpClient.InternalPost($"{_baseUrl}/api/sender/ensure-valid", receiverEmails).ConfigureAwait(false);
        }
    }
}
