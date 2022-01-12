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

        public SenderProviderProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ReceiversModel> GetAsync(Guid receiverGuid)
        {
            return await _httpClient.InternalGet<ReceiversModel>($"/api/sender/{receiverGuid:D}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<ReceiversModel>> GetAsync()
        {
            return await _httpClient.InternalGet<IEnumerable<ReceiversModel>>("/api/sender").ConfigureAwait(false);
        }

        public async Task EnsureReceiversIsValidAsync(IEnumerable<string> receiverEmails)
        {
            await _httpClient.InternalPost("/api/sender/ensure-valid", receiverEmails).ConfigureAwait(false);
        }
    }
}
