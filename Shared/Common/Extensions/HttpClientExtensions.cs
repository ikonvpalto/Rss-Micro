using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Models;
using Newtonsoft.Json;

namespace Common.Extensions
{
    public static class HttpClientExtensions
    {
        private const string JsonMediaType = "application/json";

        public static async Task InternalPost<TBody>(this HttpClient httpClient, string url, TBody body)
        {
            var message = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, JsonMediaType),
                Method = HttpMethod.Post,
                RequestUri = new Uri(url)
            };

            await httpClient.SendInternalRequest(message).ConfigureAwait(false);
        }

        public static async Task<TResponse> InternalPost<TResponse>(this HttpClient httpClient, string url)
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url)
            };

            var response = await httpClient.SendInternalRequest(message).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TResponse>(response);
        }

        public static async Task InternalPut<TBody>(this HttpClient httpClient, string url, TBody body)
        {
            var message = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, JsonMediaType),
                Method = HttpMethod.Put,
                RequestUri = new Uri(url)
            };

            await httpClient.SendInternalRequest(message).ConfigureAwait(false);
        }

        public static async Task InternalDelete(this HttpClient httpClient, string url)
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(url)
            };

            await httpClient.SendInternalRequest(message).ConfigureAwait(false);
        }

        public static async Task<TResponse> InternalGet<TResponse>(this HttpClient httpClient, string url)
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            var response = await httpClient.SendInternalRequest(message).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TResponse>(response);
        }

        public static async Task InternalGet(this HttpClient httpClient, string url)
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            await httpClient.SendInternalRequest(message).ConfigureAwait(false);
        }

        private static async Task<string> SendInternalRequest(this HttpClient httpClient, HttpRequestMessage message)
        {
            var response = await httpClient.SendAsync(message).ConfigureAwait(false);
            var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new BaseHttpException(
                    JsonConvert.DeserializeObject<ErrorResponse>(stringResponse)?.ErrorMessage ?? string.Empty,
                    response.StatusCode);
            }

            return stringResponse;
        }
    }
}
