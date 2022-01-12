using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Models;
using Common.Resources;
using Newtonsoft.Json;

namespace Common.Extensions
{
    public static class HttpClientExtensions
    {
        private const string JsonMediaType = "application/json";

        public static async Task InternalPost(this HttpClient httpClient, string url, object? body = null)
        {
            var message = new HttpRequestMessage
            {
                Content = body is null
                    ? null
                    : new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, JsonMediaType),
                Method = HttpMethod.Post,
                RequestUri = new (url, UriKind.Relative)
            };

            await httpClient.SendInternalRequest(message).ConfigureAwait(false);
        }

        public static async Task<TResponse> InternalPost<TResponse>(this HttpClient httpClient, string url, object? body = null)
        {
            var message = new HttpRequestMessage
            {
                Content = body is null
                    ? null
                    : new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, JsonMediaType),
                Method = HttpMethod.Post,
                RequestUri = new (url, UriKind.Relative)
            };

            return await httpClient.SendInternalRequest<TResponse>(message).ConfigureAwait(false);
        }

        public static async Task InternalPut<TBody>(this HttpClient httpClient, string url, TBody body)
        {
            var message = new HttpRequestMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, JsonMediaType),
                Method = HttpMethod.Put,
                RequestUri = new (url, UriKind.Relative)
            };

            await httpClient.SendInternalRequest(message).ConfigureAwait(false);
        }

        public static async Task InternalDelete(this HttpClient httpClient, string url)
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new (url, UriKind.Relative)
            };

            await httpClient.SendInternalRequest(message).ConfigureAwait(false);
        }

        public static async Task<TResponse> InternalGet<TResponse>(this HttpClient httpClient, string url)
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new (url, UriKind.Relative)
            };

            return await httpClient.SendInternalRequest<TResponse>(message).ConfigureAwait(false);
        }

        public static async Task InternalGet(this HttpClient httpClient, string url)
        {
            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new (url, UriKind.Relative)
            };

            await httpClient.SendInternalRequest(message).ConfigureAwait(false);
        }

        public static async Task<T?> ParseNullableBodyAsync<T>(this HttpResponseMessage response)
        {
            return await ParseNullableJsonBodyAsync<T>(response).ConfigureAwait(false);
        }

        public static async Task<T> ParseBodyAsync<T>(this HttpResponseMessage response)
        {
            return await ParseJsonBodyAsync<T>(response).ConfigureAwait(false);
        }

        private static async Task<T> SendInternalRequest<T>(this HttpClient httpClient, HttpRequestMessage message)
        {
            var response = await SendInternalRequest(httpClient, message).ConfigureAwait(false);
            return await ParseJsonBodyAsync<T>(response).ConfigureAwait(false);
        }

        private static async Task<HttpResponseMessage> SendInternalRequest(this HttpClient httpClient, HttpRequestMessage message)
        {
            var response = await httpClient.SendAsync(message).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var error = await ParseNullableJsonBodyAsync<ErrorResponse>(response).ConfigureAwait(false);
                throw BaseHttpException.Create(
                    error?.ErrorMessage ?? string.Empty,
                    response.StatusCode);
            }

            return response;
        }

        private static async Task<T> ParseJsonBodyAsync<T>(HttpResponseMessage response)
        {
            var result = await ParseNullableJsonBodyAsync<T>(response).ConfigureAwait(false);
            if (result is null)
            {
                var detailedMessage = string.Format(
                    Localization.CanNotParseBody,
                    response.RequestMessage?.RequestUri,
                    response.Content.Headers.ContentType,
                    typeof(T));

                throw new ServerInnerException(Localization.ServerInternalDefaultError,
                                        new JsonException(detailedMessage));
            }

            return result;
        }

        private static async Task<T?> ParseNullableJsonBodyAsync<T>(HttpResponseMessage response)
        {
            var stringBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if ((!response.Content.Headers.ContentType?.MediaType?.Contains("json")) ?? false)
            {
                throw new ServerInnerException(
                    $"Auth0 response not in json format: {response.Content.Headers.ContentType?.MediaType}, body: {stringBody}");
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(stringBody);
            }
            catch (JsonReaderException e)
            {
                throw new JsonException($"Can't parse {stringBody}", e);
            }
        }
    }
}
