using Auth.Common.Contracts;
using Auth.Common.Models;
using Common.Extensions;

namespace Auth.Facade.Proxies;

public sealed class AuthServiceProxy : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthServiceProxy(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetLoginUrlAsync()
    {
        return await _httpClient.InternalGet<string>("login/url").ConfigureAwait(false);
    }

    public async Task<string> GetLogoutUrlAsync()
    {
        return await _httpClient.InternalGet<string>("logout/url").ConfigureAwait(false);
    }

    public async Task<UserProfile> GetUserProfileAsync(string token)
    {
        throw new NotImplementedException();
    }
}
