using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Extensions;
using Downloader.Common.Models;
using Sender.Common.Contracts;
using Sender.Common.Models;

namespace Sender.Facade.HttpProxy
{
    public sealed class SenderManagerProxy : ISenderManager
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public SenderManagerProxy(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
        }

        public async Task CreateAsync(ReceiversModel receivers)
        {
            await _httpClient.InternalPost($"{_baseUrl}/api/sender", receivers).ConfigureAwait(false);
        }

        public async Task UpdateAsync(ReceiversModel receivers)
        {
            await _httpClient.InternalPut($"{_baseUrl}/api/sender", receivers).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid receiversGuid)
        {
            await _httpClient.InternalDelete($"{_baseUrl}/api/sender/{receiversGuid:D}").ConfigureAwait(false);
        }

        public async Task SendNewsAsync(Guid receiversGuid, IEnumerable<NewsItem> news)
        {
            await _httpClient.InternalPost($"{_baseUrl}/api/sender/{receiversGuid:D}/send-news", news).ConfigureAwait(false);
        }
    }
}
