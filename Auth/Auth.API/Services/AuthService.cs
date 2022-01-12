using System;
using System.Net.Http;
using System.Threading.Tasks;
using Auth.API.Exceptions;
using Auth.API.Models;
using Auth.API.Resources;
using Auth.API.Sections;
using Auth.Common.Contracts;
using Auth.Common.Models;
using Common.Exceptions;
using Common.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Auth.API.Services
{
    public sealed class AuthService : IAuthService
    {
        private const string UserProfilePath = "/userinfo";
        private const string LogoutPath = "/v2/logout";
        private const string LoginPath = "/authorize";
        private const string JsonContentType = "application/json";

        private readonly IOptions<AuthSection> _authSection;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IOptions<AuthSection> authSection,
            HttpClient httpClient,
            ILogger<AuthService> logger)
        {
            _authSection = authSection;
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new (_authSection.Value.PreparedDomain);
        }

        public Task<string> GetLoginUrlAsync()
        {
            return Task.FromResult($"{_authSection.Value.PreparedDomain}{LoginPath}" +
                                   "?response_type=token" +
                                   $"&client_id={_authSection.Value.ClientId}" +
                                   $"&redirect_uri={_authSection.Value.CallbackHost}/login/callback" +
                                   "&scope=openid%20profile%20email");
        }

        public Task<string> GetLogoutUrlAsync()
        {
            return Task.FromResult($"{_authSection.Value.PreparedDomain}{LogoutPath}" +
                                   $"?client_id={_authSection.Value.ClientId}" +
                                   $"&returnTo={_authSection.Value.CallbackHost}/logout/callback");
        }

        public async Task<UserProfile> GetUserProfileAsync(string token)
        {
            try
            {
                return await SendAuth0RequestAsync<UserProfile>(UserProfilePath, token).ConfigureAwait(false);
            }
            catch (Auth0Exception e)
            {
                throw new ServerInnerException(Localization.CanNotGetUserProfile, e);
            }
        }

        private async Task<T> SendAuth0RequestAsync<T>(string path, string? token = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new (path, UriKind.Relative),
                Headers =
                {
                    { "Accept", JsonContentType }
                }
            };

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Add("Authorization", $"Bearer {token}");
            }

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.ParseBodyAsync<Auth0ErrorModel>();
                throw new Auth0Exception(error, Localization.Auth0RequestFailed, path);
            }

            return await response.ParseBodyAsync<T>();
        }
    }
}
